using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using experses.Models;
using ExpensesEF;
using System.Globalization;
using expenses.Models;

namespace expenses.Controllers
{
    public class SueldosController : Controller
    {
        // GET: Sueldos
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult UpdateSueldo()
        {
            //var model = new expenses.Models.SueldosViewModel();
            var context = new ExpensesEF.Entities();
            
            string _IdUser;

            _IdUser = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();

            Sueldo sueldo = context.Sueldo.Where(x => x.idUserGasto == _IdUser).FirstOrDefault();

            if (sueldo is null)
            {
                //Insert
                sueldo = new Sueldo();
                sueldo.idUserGasto = _IdUser;
                context.Entry(sueldo).State = System.Data.Entity.EntityState.Added;
            }
            else
            {
                //update
                context.Entry(sueldo).State = System.Data.Entity.EntityState.Modified;
            }

            sueldo.SueldoBrutoAnual = Utils.parseDecimal(Request.Form["txtBruto"].ToString());
            sueldo.SueldoNetoEnero = Utils.parseDecimal(Request.Form["txtEnero"].ToString());
            sueldo.SueldoNetoFebrero = Utils.parseDecimal(Request.Form["txtFebrero"].ToString());
            sueldo.SueldoNetoMarzo = Utils.parseDecimal(Request.Form["txtMarzo"].ToString());
            sueldo.SueldoNetoAbril = Utils.parseDecimal(Request.Form["txtAbril"].ToString());
            sueldo.SueldoNetoMayo = Utils.parseDecimal(Request.Form["txtMayo"].ToString());
            sueldo.SueldoNetoJunio = Utils.parseDecimal(Request.Form["txtJunio"].ToString());
            sueldo.SueldoNetoJulio = Utils.parseDecimal(Request.Form["txtJulio"].ToString());
            sueldo.SueldoNetoAgosto = Utils.parseDecimal(Request.Form["txtAgosto"].ToString());
            sueldo.SueldoNetoSeptiembre = Utils.parseDecimal(Request.Form["txtSeptiembre"].ToString());
            sueldo.SueldoNetoOctubre = Utils.parseDecimal(Request.Form["txtOctubre"].ToString());
            sueldo.SueldoNetoNoviembre = Utils.parseDecimal(Request.Form["txtNoviembre"].ToString());
            sueldo.SueldoNetoDiciembre = Utils.parseDecimal(Request.Form["txtDiciembre"].ToString());


            context.SaveChanges();
                                   
            return RedirectToAction("Index", "Manage");
        }

      
    }
}