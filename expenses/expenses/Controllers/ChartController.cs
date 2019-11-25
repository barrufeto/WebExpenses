using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using experses.Models;


namespace expenses.Controllers
{
    public class ChartController : Controller
    {

        

        // GET: Chart
        public ActionResult Index()
        {
            //model._TipoChart = 0;
            ChartViewModel m = new ChartViewModel();
            m._TipoChart = 0;
            return View(m);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(ChartViewModel e)
        {

            return View(e);
        }


        public ActionResult Test()
        {
            return View();
        }




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

        }
    }
}