using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using experses.Models;
using System.Web.Helpers;

namespace experses.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {

            //Si no ets autenticat, ves a la home.
            if (Request.IsAuthenticated)
                return RedirectToAction("IndexA", "Home");


            return View();
        }


        public ActionResult IndexA()
        {

            if (Request.IsAuthenticated)
            {
                //Si no ets autenticat, ves a la home.
                var context = new ExpensesEF.Entities();
                var model = new HomeViewModels();
                string _IdUser;
                decimal sueldoactual;

                _IdUser = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
                System.Data.Entity.Core.Objects.ObjectParameter myExpensesThisMonth = new System.Data.Entity.Core.Objects.ObjectParameter("RetMont", typeof(Decimal));
                System.Data.Entity.Core.Objects.ObjectParameter myExpensesLastMonth = new System.Data.Entity.Core.Objects.ObjectParameter("RetMontPrevious", typeof(Decimal));
                System.Data.Entity.Core.Objects.ObjectParameter myExpensesThisMonthCard = new System.Data.Entity.Core.Objects.ObjectParameter("RetCurrentCard", typeof(Decimal));
                System.Data.Entity.Core.Objects.ObjectParameter myExpensesThisMonthIncludingNonComputables = new System.Data.Entity.Core.Objects.ObjectParameter("RetMontIncludingComputables", typeof(Decimal));

                context.spGetMonthlyExpenses(context.AspNetUsers.Where(x => x.Id == _IdUser).FirstOrDefault().Id.ToString(), DateTime.Now.Month, DateTime.Now.Year, myExpensesThisMonth, myExpensesLastMonth, myExpensesThisMonthCard, myExpensesThisMonthIncludingNonComputables);

                if (String.IsNullOrEmpty(myExpensesThisMonth.Value.ToString())) { model.TotalGastoMensual = 0; }
                else
                { model.TotalGastoMensual = Math.Round(Convert.ToDecimal(myExpensesThisMonth.Value.ToString()), 2); }

                if (String.IsNullOrEmpty(myExpensesThisMonth.Value.ToString())) { model.TotalGastoMensualIncludingNonComputables = 0; }
                else
                { model.TotalGastoMensualIncludingNonComputables = Math.Round(Convert.ToDecimal(myExpensesThisMonthIncludingNonComputables.Value.ToString()), 2); }
                


                if (String.IsNullOrEmpty(myExpensesLastMonth.Value.ToString())) { model.TotalGastoMensualMesAnterior = 0; }
                else
                { model.TotalGastoMensualMesAnterior = Math.Round(Convert.ToDecimal(myExpensesLastMonth.Value.ToString()), 2); }

                if (String.IsNullOrEmpty(myExpensesThisMonthCard.Value.ToString())) { model.TotalGastoMensualTarjeta = 0; }
                else
                { model.TotalGastoMensualTarjeta = Math.Round(Convert.ToDecimal(myExpensesThisMonthCard.Value.ToString()), 2); }


                if (String.IsNullOrEmpty(myExpensesThisMonth.Value.ToString()) || (String.IsNullOrEmpty(myExpensesLastMonth.Value.ToString())))
                    model.tpcRespectoMesAnterior = 0;
                else
                { model.tpcRespectoMesAnterior = Math.Round((Math.Round(Convert.ToDecimal(myExpensesThisMonth.Value.ToString()), 2) / Math.Round(Convert.ToDecimal(myExpensesLastMonth.Value.ToString()), 2)) * 100); }


                model.EnabletpcRespectoMesAnterior = context.AspNetUsers.Where(u => u.Id == _IdUser).FirstOrDefault().VerTpcGastovsMesAnterior.GetValueOrDefault();


                model.SueldoPrevisto = _getSueldoActual(context, _IdUser);
                model.AhorroPrevisoMesActual = (model.SueldoPrevisto > 0 ? model.SueldoPrevisto - model.TotalGastoMensualIncludingNonComputables : 0);

                if (Request.Browser.IsMobileDevice == true) {  model.isMobile = true; }

               
                return View(model);
            }
                
            else
                return RedirectToAction("Index", "Home");

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private decimal _getSueldoActual(ExpensesEF.Entities context, string _IdUser)
        {
            decimal _sueldo=0;

            try
            {
                switch (DateTime.Now.Month)
                {
                    case 1:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoEnero.GetValueOrDefault();
                        break;
                    case 2:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoFebrero.GetValueOrDefault();
                        break;
                    case 3:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoMarzo.GetValueOrDefault();
                        break;
                    case 4:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoAbril.GetValueOrDefault();
                        break;
                    case 5:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoMayo.GetValueOrDefault();
                        break;
                    case 6:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoJunio.GetValueOrDefault();
                        break;
                    case 7:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoJulio.GetValueOrDefault();
                        break;
                    case 8:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoAgosto.GetValueOrDefault();
                        break;
                    case 9:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoSeptiembre.GetValueOrDefault();
                        break;
                    case 10:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoOctubre.GetValueOrDefault();
                        break;
                    case 11:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoNoviembre.GetValueOrDefault();
                        break;
                    case 12:
                        _sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault().SueldoNetoDiciembre.GetValueOrDefault();
                        break;
                }
            }
            catch (Exception _ex)
            {
                _sueldo = 0;
            }


            return _sueldo;
        }

      



    }
}

