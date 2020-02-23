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

        public ActionResult RptGastoTipo()
        {
            string _codigo;
            var model = new RptTipoModel();

            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);
            using (var context = new ExpensesEF.Entities())
            {
                model._GrupoGastosTrad = GetListGrupoGastos(context.GrupoGastoTextosTraduccion.Where(x => (x.GrupoGasto.VisibleReports==true) && (x.GrupoGasto.idTipoGrupoGasto==2) && (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).ToList().OrderBy(x => x.Descripcion));

            }
            return View(model);

        }


        public ActionResult RptTopTenSubtipo()
        {
            string _codigo;
            var model = new RptTipoModel();

            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);
            using (var context = new ExpensesEF.Entities())
            {
                model._TipoGastosTrad = GetListTipoGastos(context.TipoGastoTextosTraduccion.Where(x => (x.TipoGasto.Activo == true) && (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).ToList().OrderBy(x => x.Descripcion));

            }
                return View(model);
        }

        #region "Pantalla TopTen"

        public ActionResult GetSubTiposTipo(int id)
        {
            var context = new ExpensesEF.Entities();
            IEnumerable<SelectListItem> lista;
            string _codigo;
            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);


            lista = GetSubTiposGastos(context.SubTipoGastoTextosTraduccion.Where(x => x.SubTipoGasto.idTipoGasto == id && x.SubTipoGasto.Activo == true && x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma).OrderBy(x => x.Descripcion));

            return Json(lista.Select(x => new
            {
                idSubTipo = x.Value,
                NombreSubTipo = x.Text
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<SelectListItem> GetSubTiposGastos(IEnumerable<SubTipoGastoTextosTraduccion> elements)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            // For each string in the 'elements' variable, create a new SelectListItem object
            // that has both its Value and Text properties set to a particular value.
            // This will result in MVC rendering each item as:
            //     <option value="State Name">State Name</option>
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.idSubTipoGasto.ToString(),
                    Text = element.Descripcion
                });
            }

            return selectList;
        }

               

        private IEnumerable<SelectListItem> GetListGrupoGastos(IEnumerable<GrupoGastoTextosTraduccion> elements)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            // For each string in the 'elements' variable, create a new SelectListItem object
            // that has both its Value and Text properties set to a particular value.
            // This will result in MVC rendering each item as:
            //     <option value="State Name">State Name</option>
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.idGrupoGasto.ToString(),
                    Text = element.Descripcion
                });
            }

            return selectList;
        }

        private IEnumerable<SelectListItem> GetListTipoGastos(IEnumerable<TipoGastoTextosTraduccion> elements)
            {
                // Create an empty list to hold result of the operation
                var selectList = new List<SelectListItem>();

                // For each string in the 'elements' variable, create a new SelectListItem object
                // that has both its Value and Text properties set to a particular value.
                // This will result in MVC rendering each item as:
                //     <option value="State Name">State Name</option>
                foreach (var element in elements)
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = element.idTipoGasto.ToString(),
                        Text = element.Descripcion
                    });
                }

                return selectList;
            }

        #endregion

        #region "Dades de reports"

        public JsonResult GetResumenTipoGasto( int IdGrupo)
        {
            var context = new ExpensesEF.Entities();
            string _User = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
            List<object> chartData = new List<object>();
            //var datos = context.SubTipoGasto.Where(x=> x.idTipoGasto == id).ToList();


            if (IdGrupo == 0)
            {
                chartData.Add(new object[]
                                {
                            "Grupo Gasto", "Dinero gastado(€)"
                                });

                var datos = context.Gasto.Where(x => (x.idUserGasto == _User) && (x.GrupoGasto.VisibleReports == true) && (x.GrupoGasto.idTipoGrupoGasto == 2)).GroupBy(y => y.idGrupoGasto).Select(a => new { idGrupoGasto = a.Key, Amount = a.Sum(b => b.Precio) });


                foreach (var Elem in datos)
                {
                    if (Elem.idGrupoGasto != null)
                    {
                        chartData.Add(new object[]
                        {
                          context.GrupoGastoTextosTraduccion.Where( x=>(x.idGrupoGasto == Elem.idGrupoGasto) && (x.idIdioma==1) &&  (x.GrupoGasto.VisibleReports==true) && (x.GrupoGasto.idTipoGrupoGasto==2)).FirstOrDefault().Descripcion, Elem.Amount
                        });
                    }

                }
            }
            else
            {
                chartData.Add(new object[]
                                {
                            "Tipo Gasto", "Dinero gastado(€)"
                                });
                var datos = context.Gasto.Where(x => x.idUserGasto == _User && x.idGrupoGasto == IdGrupo).GroupBy(y => y.idTipoGasto).Select(a => new { idTipoGasto = a.Key, Amount = a.Sum(b => b.Precio) });


                foreach (var Elem in datos)
                {
                        chartData.Add(new object[]
                        {
                          context.TipoGastoTextosTraduccion.Where( x=>x.idTipoGasto == Elem.idTipoGasto && x.idIdioma==1).FirstOrDefault().Descripcion, Elem.Amount
                        });
                }
            }



            return Json(chartData);
        }


        public JsonResult GetTopTenData(int id)
        {
            var context = new ExpensesEF.Entities();
            string _User = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();

            //var datos = context.Gasto.Where(x => x.idUserGasto == _User && x.idSubTipoGasto==24).OrderBy(x => x.concepto).ThenByDescending(x => x.Concepto).GroupBy(y=>y.Concepto).ToList();
            var datos = context.Gasto.Where(x => x.idUserGasto == _User && x.idSubTipoGasto == id).GroupBy(y => y.Concepto).OrderByDescending(y=>y.Count()).ToList().Take(10);

            List<object> chartData = new List<object>();
            chartData.Add(new object[]
                            {
                            "Concepto", "Veces", "Dinero gastado(€)"
                            });


            foreach (var Elem in datos)
            {
                     chartData.Add(new object[]
                             {
                                 Elem.Key, Elem.Count(), context.Gasto.Where(x => x.idUserGasto == _User && x.idSubTipoGasto == id && x.Concepto == Elem.Key).Sum(x=>x.Precio)
            });
                 
            }
            return Json(chartData);

        }
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