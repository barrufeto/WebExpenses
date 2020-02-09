using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpensesEF;
using System.Net;
using System.Net.Mail;

namespace expenses.Models
{
    public class HangFireController : Controller
    {
        // GET: HangFire
        public ActionResult Index()
        {
            return View();
        }
       
        //Enviament emails 
        public ActionResult SendMonthlyEmails()
        {

            Emails emails = new Emails();
            emails.Send();
            return null;    
        }

        //Resums (Mensual i Anuals
        //Execució mensual
        public ActionResult ResumsGastos()
        {
            var context = new ExpensesEF.Entities();
            int year;
            int month;

            //mirem per executar-ho al mes anterior
            if (DateTime.Now.Month == 1)
            {
                month = 12;
                year = DateTime.Now.Year - 1;
            }
            else
            {
                month = DateTime.Now.Month-1;
                year = DateTime.Now.Year;
            }

            context.spSummarizeExpenses(month, year);

            //Tots els gastos del mes anterior, es posen a zero.

            return null;
        }

        //Acumulat gasto anual per persona
        //Cada dia a 23:59
        public ActionResult AcumulatedGastos()
        {
            var context = new ExpensesEF.Entities();
            int year;
            year = DateTime.Now.Year;
            context.spGastosDiarioAcumulados(year);
            return null;
        }

            //Gastos programats
            //Execució diària
            public ActionResult SchedulesGastos()
        {
            var context = new ExpensesEF.Entities();

            List<GastoRecurrente> _GastoRecurrentes;

            _GastoRecurrentes = context.GastoRecurrente.Where(x => x.Activo != 0).ToList();
            var dbContextTransaction = context.Database.BeginTransaction();

            try
            {
                //Per a cada gasto "Recurrent"
                foreach (GastoRecurrente _GastoRecurrente in _GastoRecurrentes)
                {
                    //Comparem sempre la data de la propera execució. Si és la data actual, creem el nou gasto.
                    if (_GastoRecurrente.SiguienteEjecucion.Year == DateTime.Now.Year && _GastoRecurrente.SiguienteEjecucion.Month == DateTime.Now.Month && _GastoRecurrente.SiguienteEjecucion.Day == DateTime.Now.Day && _GastoRecurrente.Activo == 1)
                    {

                        //Creem el gasto
                        Gasto _Gasto = new Gasto();
                        _Gasto.Concepto = _GetConcepto(_GastoRecurrente);
                        _Gasto.idUserGasto = _GastoRecurrente.idUserGastoRecurrente;
                        _Gasto.idTipoGasto = _GastoRecurrente.idTipoGastoRecurrente;
                        _Gasto.Fecha = DateTime.Now;
                        _Gasto.Precio = _GastoRecurrente.Precio;
                        _Gasto.idTipoPago = _GastoRecurrente.idTipoPago;
                        _Gasto.GastoComputable = (_GastoRecurrente.GastoComputable ? 1 : 0);
                        _Gasto.GastoRecurrente = true;
                        _Gasto.Resaltar = true;
                        _Gasto.GastoEditable = true;
                        _Gasto.EsCompartido = false;
                        _Gasto.idSubTipoGasto = _GastoRecurrente.idSubTipoGasto;
                        _Gasto.EsRegalo = false;


                        //Update amb la data de nova execució
                        _GastoRecurrente.SiguienteEjecucion = _GastoRecurrente.SiguienteEjecucion.AddMonths(_GastoRecurrente.Periocidad1.MesesASumar.GetValueOrDefault());

                        //Enquem els canvis
                        context.Entry(_Gasto).State = System.Data.Entity.EntityState.Added;
                        context.Entry(_GastoRecurrente).State = System.Data.Entity.EntityState.Modified;

                        //Fem els canvis a la BBDD
                        context.SaveChanges();
                    }


                }
                dbContextTransaction.Commit();
            }
            catch (Exception _Ex)
            {
                dbContextTransaction.Rollback();
            }
            return null;
        }




        //ens retorna un concepte ja formatejat
        private string _GetConcepto(GastoRecurrente _GastoRecurrente)
        {
            switch (_GastoRecurrente.Periocidad1.MesesASumar)
            {
                case 1:
                    return _GastoRecurrente.Concepto + " " + DateTime.Now.Month.ToString().PadLeft(2, '0') + '/' + DateTime.Now.Year;
                case 2:
                    return _GastoRecurrente.Concepto;
                case 3:
                    return _GastoRecurrente.Concepto;
                case 12:
                    return _GastoRecurrente.Concepto + " " + (DateTime.Now.Year) + " - " + (DateTime.Now.Year)+1.ToString();
                case 24:
                    return _GastoRecurrente.Concepto + " " +(DateTime.Now.Year) + " - " + +(DateTime.Now.Year) + 2.ToString(); 
                default:
                    return _GastoRecurrente.Concepto;
            }
            
        }


    }
}