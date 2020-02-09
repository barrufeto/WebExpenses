using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using experses.Models;
using ExpensesEF;


namespace expenses.Controllers
{
    public class ChartController : Controller
    {

        

        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RptAhorroGasto()
        {
            return View();
        }

        public ActionResult RptGastoMensual()
        {
            var context = new ExpensesEF.Entities();
            experses.Models.ChartGastoMensual model = new experses.Models.ChartGastoMensual();

            DateTime dtAnterior = new DateTime();
            dtAnterior = DateTime.Now.AddMonths(-1);

            string _User = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();

            model.GastoMesActual = context.Gasto.Where(x=>x.idUserGasto == _User && x.Fecha.Year == DateTime.Now.Year && x.Fecha.Month == DateTime.Now.Month).Sum(x=>x.Precio);
            model.GastoMesAnterior = context.Gasto.Where(x => x.idUserGasto == _User && x.Fecha.Year == dtAnterior.Year && x.Fecha.Month == dtAnterior.Month).Sum(x => x.Precio);

            return View(model);
        }

        public  ActionResult RptGastoAcumulado()
        {
            return View();
        }

        public ActionResult RptDiffEntreMeses()
        {
            return View();
        }

        

        /*
        [Authorize]
        [HttpGet]
        public ActionResult GetChart(ChartViewModel e)
        {
            string user;
            string _idioma;
            int iHeight, iWidth;
            var context = new ExpensesEF.Entities();
            List<string> xValue = new List<string>();
            List<string> yValue = new List<string>();
            string _Title="";
            string _StyleChart = "";

            user = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
            _idioma = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);

            //string _test;
            //_test = Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(“iPad”);


            switch (e._EstiloChart)
            {
                case ChartViewModel.eEstiloChart.Barras:
                    _StyleChart = "Column";
                    break;

                case ChartViewModel.eEstiloChart.Quesito:
                    _StyleChart = "Pie";
                    break;
            }

                    switch (e._TipoChart)
            {
                case ChartViewModel.eTipoChart.MesActual:
                    yValue = context.Gasto.Where(x => (x.Fecha.Month == DateTime.Now.Month && x.Fecha.Year == DateTime.Now.Year && x.idUserGasto == user)).GroupBy(item => item.idTipoGasto).Select(group => group.Sum(item => item.Precio)).Cast<string>().ToList();
                    xValue = context.Gasto.Where(x => (x.Fecha.Month == DateTime.Now.Month && x.Fecha.Year == DateTime.Now.Year && x.idUserGasto == user)).GroupBy(item => item.idTipoGasto).Select(group =>group.FirstOrDefault().TipoGasto.TipoGastoTextosTraduccion.Where( u=>u.Idiomas.codigo==_idioma).FirstOrDefault().Descripcion).Cast<string>().ToList();
                    _Title = expenses.Resources.Descripciones.GraficoMesActual;
                    break;

                case ChartViewModel.eTipoChart.AnyoActual:
                    yValue = context.Gasto.Where(x => (x.Fecha.Year == DateTime.Now.Year && x.idUserGasto == user)).GroupBy(item => item.idTipoGasto).Select(group => group.Sum(item => item.Precio)).Cast<string>().ToList();
                    xValue = context.Gasto.Where(x => (x.Fecha.Year == DateTime.Now.Year && x.idUserGasto == user)).GroupBy(item => item.idTipoGasto).Select(group => group.FirstOrDefault().TipoGasto.TipoGastoTextosTraduccion.Where(u => u.Idiomas.codigo == _idioma).FirstOrDefault().Descripcion).Cast<string>().ToList();
                    _Title = expenses.Resources.Descripciones.GraficoAnyoActual;
                    break;
            }



            bool ismobile = false;
            iHeight = 0;
            iWidth = 0;

            int screenwidth = Request.Browser.ScreenPixelsWidth; //Always returns 640 ?
            if (Request.Browser.IsMobileDevice == true) { ismobile = true; } //Doesn't detect mobile device ?

            if (ismobile)
            {
                
                if (Request.ServerVariables["HTTP_USER_AGENT"].Contains("iPad"))
                { //IPAD
                    iWidth = 720;
                    iHeight = 400;
                }
                else {
                    //Telèfons
                    iWidth = 350;
                    iHeight = 200;
                    }
            }
            else //Resto
            {
                iWidth = 1150;
                iHeight = 400;
            }

            new Chart(iWidth, iHeight, ChartTheme.Blue)
                  .AddTitle(_Title)
                  //.AddLegend()
                  .AddSeries(
                      name: "Gastos",
                      chartType: _StyleChart,
                      xValue: xValue,
                      yValues: yValue)
                    .Write("png");
            return null;

        } */

        #region "Dades de reports"

        public JsonResult GetGastoAcumuladoData()
        {
            var context = new ExpensesEF.Entities();
            string _User = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
            
            var datos = context.SummaryGastoAcumuladoPorDia.Where(x => x.idUser == _User && (x.Fecha.Year == DateTime.Now.Year)).ToList();

            List<object> chartData = new List<object>();
            chartData.Add(new object[]
                            {
                            "Dia", expenses.Resources.Descripciones.RptGastoAcumuladoLabel2
                            });

            foreach (var Elem in datos)
            {
                chartData.Add(new object[]
                        {
                            Elem.DayInYear, Elem.GastoAcumuladoAnual
                        });
            }


            return Json(chartData);

        }

        public JsonResult GetGastoMensualData()
        {
            var context = new ExpensesEF.Entities();
            string _User = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
            string _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);

            var datos = context.Gasto.Where(x => x.idUserGasto == _User && (x.Fecha.Year == DateTime.Now.Year && x.Fecha.Month == DateTime.Now.Month)).GroupBy(y => y.idTipoGasto).Select(group => new
                {
                    idTipoGasto = group.Key,          
                    Precio = group.Sum(x => x.Precio)
                })
                .ToList();

            

            List<object> chartData = new List<object>();
            chartData.Add(new object[]
                            {
                            "TipoGasto", "Gasto"
                            });
            
            foreach (var Elem in datos)
            {
                chartData.Add(new object[]
                        {
                            context.TipoGastoTextosTraduccion.Where (x=>x.idTipoGasto == Elem.idTipoGasto && (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma) ).FirstOrDefault().Descripcion.ToString(), Elem.Precio
                        });
            }
            

            return Json(chartData);
        }

        private List<object> GeneraDatesDataComparadoReport(string _User, int _Year, int _Month)
        {
            var context = new ExpensesEF.Entities();
            List<object> chartData = new List<object>();
            
            string _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);
            List<TipoGasto> TiposGasto = context.TipoGasto.Where(x => x.Activo == true).ToList();

            var datos = context.Gasto.Where(x => x.idUserGasto == _User && (x.Fecha.Year == _Year && x.Fecha.Month == _Month)).GroupBy(y => y.idTipoGasto).Select(group => new
            {
                idTipoGasto = group.Key,
                Precio = group.Sum(x => x.Precio)
            })
              .ToList();

            chartData.Add(new object[]
                            {
                            "TipoGasto", expenses.Resources.Descripciones.RptGastoMensualGraph2
                            });

            //Contruïm les dades del gràfic actual
            foreach (var Elem in TiposGasto)
            {
                var find = datos.FirstOrDefault(x => x.idTipoGasto == Elem.idTipoGasto);
                if (find != null)
                {
                    chartData.Add(new object[]
                        {
                            context.TipoGastoTextosTraduccion.Where (x=>x.idTipoGasto == Elem.idTipoGasto && (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma) ).FirstOrDefault().Descripcion.ToString(), find.Precio
                        });
                }
                else
                {
                    chartData.Add(new object[]
                        {
                            context.TipoGastoTextosTraduccion.Where (x=>x.idTipoGasto == Elem.idTipoGasto && (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma) ).FirstOrDefault().Descripcion.ToString(), 0
                        });
                }
            }
            return chartData;
        }

        public JsonResult GetGastoMensualDataComparado()
        {
            var context = new ExpensesEF.Entities();
            string _User = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
                        
            List<object> chartActual = new List<object>();
            List<object> chartPrev = new List<object>();

            DateTime dt = new DateTime();
            dt = DateTime.Now.AddMonths(-1);

            chartActual = GeneraDatesDataComparadoReport(_User, DateTime.Now.Year, DateTime.Now.Month);
            chartPrev = GeneraDatesDataComparadoReport(_User, dt.Year, dt.Month);
            
            return Json(new { DatosActual = chartActual, DatosMesAnterior = chartPrev });
        }








        public JsonResult GetAhorroGastoData()
        {
            var context = new ExpensesEF.Entities();
            IEnumerable<SummaryTotal_Mensual> lista;
            string _User = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();

            DateTime dtInicio = DateTime.Now.AddMonths(-12);
            DateTime dtFin = DateTime.Now;
            string _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
                           {
                            expenses.Resources.Descripciones.RptGastoMensualGraph1, expenses.Resources.Descripciones.RptGastoMensualGraph2, expenses.Resources.Descripciones.RptGastoMensualGraph3
                           });

            lista = context.SummaryTotal_Mensual.Where(x => x.idUser == _User && ((x.Año == dtInicio.Year && x.Mes >= dtInicio.Month) || (x.Año == dtFin.Year))).OrderBy(y =>y.Año);

          
            foreach (var Elem in lista)
            {
                chartData.Add(new object[]
                        {
                            expenses.Utils.GetMesLetras(Elem.Mes)+"/"+ Elem.Año.ToString().Substring(2,2), Elem.GastoTotal,Elem.AhorroFinDeMes
                        });
            }
            return Json(chartData);
        }
        #endregion
    }
}