using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using experses.Models;
using ExpensesEF;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace experses.Controllers
{
    [Authorize]
    public class GastosController : Controller
    {

        #region "Gastos normales"
        // GET: Gastos
        public ActionResult Index()
        {

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Ver(int id)
        {
            var model = new GastosViewModels();
            model = GetDatosModel(id);

            return View(model);
        }

        [Authorize]
        [HttpGet]
        // Exportem gastos de l'any en curs
        public ActionResult ExportDatos()
        {
            using (var context = new ExpensesEF.Entities())
            {
                List<vst_Export_Gastos> _Gastos;
                string _User = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
                //_Gastos = context.vst_Export_Gastos.Where(x=>x.idUserGasto == _User && x.Fecha.Substring(6,4)==DateTime.Now.Year.ToString()).OrderBy(x=>x.Fecha).ToList();
                _Gastos = context.vst_Export_Gastos.Where(x => x.idUserGasto == _User && x.Fecha.Year >= 2019).OrderBy(x => x.Fecha).ToList();

                var gv = new GridView();
                gv.DataSource = _Gastos;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Datos.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
                return View("Index");

            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Ver()
        {

            return RedirectToAction("IndexA", "Home");
        }



        [Authorize]
        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }
         

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new GastosViewModels();
            string _codigo;
            string userId;

            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);

            using (var context = new ExpensesEF.Entities())
            {
                model._TipoGastosTrad = GetListTipoGastos(context.TipoGastoTextosTraduccion.Where(x => (x.TipoGasto.Activo == true) && (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).ToList().OrderBy(x => x.Descripcion));
                model._GrupoGastosTrad = GetListItemsGrupoGastos(context.GrupoGastoTextosTraduccion.Where(x => (x.GrupoGasto.Activo == true) && (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).ToList().OrderBy(x => x.Descripcion));
                model._PersonasACompartir = GetListItemsPersonas(context.AspNetUsers.Where(y=>y.Email!= System.Web.HttpContext.Current.User.Identity.Name).OrderBy(y => y.Email));
                
                userId = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
                model._idSelectedGrupoGasto = context.AspNetUsers.Where(u => u.Id == userId).FirstOrDefault().idGrupoGastoPorDefecto.GetValueOrDefault();
                model._idDefaultTipoPagoGasto = context.AspNetUsers.Where(u => u.Id == userId).FirstOrDefault().idTipoPagoPorDefecto.GetValueOrDefault();
                

                
                if (model._idSelectedGrupoGasto == 0)
                {
                    model._idSelectedGrupoGasto = -1;
                }

                //Si no té tipus de pagament per defecte, li posem un "1" (efectiu)
                if (model._idDefaultTipoPagoGasto == 0)
                {
                    model._idSelectedTipoPagoGasto = 1;
                }
                else
                {
                    model._idSelectedTipoPagoGasto = model._idDefaultTipoPagoGasto;
                }

                
                model._TipoPagoList = context.TipoPagoTextosTraduccion.Where(x => (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).OrderBy(x => x.Descripcion).ToList();
                
                model.Fecha = DateTime.Now;
                model._FormatoFecha = GetFormatoFecha();
            }

            return View(model);
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult Add(GastosViewModels model, string submitButton)
        {
            switch (submitButton)
            {

                case "Añadir":
                case "Add":
                    //return (AddGasto(model, (Request.Form["IdSubType"]!=""?int.Parse(Request.Form["IdSubType"]) :-1)));
                    return (AddGasto(model,  (Request.Form["IdSubType"] != "" && !(Request.Form["IdSubType"] is null) ? int.Parse(Request.Form["IdSubType"]) : -1)));

                default:
                    return RedirectToAction("IndexA", "Home");
            }
            //}
        }


        private GastosViewModels GetDatosModel(int id)
        {

            var model = new GastosViewModels();
            var _Gasto = new ExpensesEF.Gasto();

            string _codigo;
            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);


            using (var context = new ExpensesEF.Entities())
            {

                model._TipoPagoList = context.TipoPagoTextosTraduccion.Where(x => (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).OrderBy(x=>x.Descripcion).ToList();

                _Gasto = context.Gasto.Where(x => x.idGasto == id).FirstOrDefault();
                model.Concepto = _Gasto.Concepto;
                model._FormatoFecha = GetFormatoFecha();
                model.Fecha = _Gasto.Fecha;
                model.Importe = _Gasto.Precio;
                model.EsRegalo = _Gasto.EsRegalo.GetValueOrDefault();
                model._idSelectedTipoPagoGasto = _Gasto.idTipoPago.GetValueOrDefault();
                model.idUserGasto = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
                model.EsCompartido = _Gasto.EsCompartido.GetValueOrDefault();
                model.Valoracion = _Gasto.Valoracion.GetValueOrDefault();

                if (_Gasto.idGrupoGasto != null)
                {
                    model._DescripcionGrupoGasto = context.GrupoGastoTextosTraduccion.Where(x => x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma && x.idGrupoGasto == _Gasto.idGrupoGasto).FirstOrDefault().Descripcion.ToString();
                }

                model._DescripcionTipoGasto = context.TipoGastoTextosTraduccion.Where(x => x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma && x.idTipoGasto == _Gasto.idTipoGasto).FirstOrDefault().Descripcion.ToString();

                if (_Gasto.idSubTipoGasto != null)
                {
                    model._DescripcionSubTipoGasto = context.SubTipoGastoTextosTraduccion.Where(x => x.idIdioma==context.Idiomas.Where(y=>y.codigo == _codigo).FirstOrDefault().idIdioma &&  x.idSubTipoGasto == _Gasto.idSubTipoGasto).FirstOrDefault().Descripcion.ToString();
                }
            }

            return model;

            
        }


        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new GastosViewModels();
            model = GetDatosModel(id);

            return View(model);
            
        }

        [Authorize]
        public ActionResult Copy(int id)
        {
            var _Gasto = new ExpensesEF.Gasto();
            var _GastoCopy = new ExpensesEF.Gasto();

            using (var context = new ExpensesEF.Entities())
            {

                _Gasto = context.Gasto.Where(x => x.idGasto == id).FirstOrDefault();

                //_GastoCopy = _Gasto;
                _GastoCopy.Concepto = _Gasto.Concepto;
                _GastoCopy.Fecha = DateTime.Now;
                _GastoCopy.GastoComputable = _Gasto.GastoComputable;
                _GastoCopy.GastoRecurrente = _Gasto.GastoRecurrente;
                _GastoCopy.idGrupoGasto = _Gasto.idGrupoGasto;
                _GastoCopy.idTipoGasto = _Gasto.idTipoGasto;
                _GastoCopy.idTipoPago = _Gasto.idTipoPago;
                _GastoCopy.idTipoMoneda = _GastoCopy.idTipoMoneda;
                _GastoCopy.idUserGasto = _Gasto.idUserGasto;
                _GastoCopy.Precio = _Gasto.Precio;
                _GastoCopy.GastoEditable = true;
                _GastoCopy.Resaltar = _Gasto.Resaltar;
                _GastoCopy.idSubTipoGasto = _Gasto.idSubTipoGasto;
                _GastoCopy.EsCompartido = false;
                _GastoCopy.EsRegalo = false;
                _GastoCopy.Valoracion = _Gasto.Valoracion;

                //No copiem si és un regalo o no

                context.Entry(_GastoCopy).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
             
            return RedirectToAction("IndexA", "Home");
        }



        [Authorize]
        [HttpPost]
        public ActionResult Edit(GastosViewModels model, int id, string submitButton)
        {
            switch (submitButton)
            {
                case "Update":
                case "Modificar":
                    return UpdateGasto(model, id);
                default:
                    return RedirectToAction("IndexA", "Home");
            }

        }


        [Authorize]
        public ActionResult EditGastoRecurrente(GastosRecurrenteViewModels model)
        {
            int _idGastoRecurrente = int.Parse(Request.Form["hidden-ide"]);
            model.Importe = expenses.Utils.parseDecimal(Request.Form["txtImporte"].ToString());
            //model.GastoComputable = Request.Form[""]
            if (ModelState.IsValid)
            {
                // Save it in database
                var context = new ExpensesEF.Entities();
                //_codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);

                

                GastoRecurrente _GastoRecurrente = new GastoRecurrente();
                _GastoRecurrente = context.GastoRecurrente.Where(x => x.idGastoRecurrente == _idGastoRecurrente).FirstOrDefault();
                _GastoRecurrente.idUserGastoRecurrente = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
                _GastoRecurrente.Concepto = model.Concepto;
                _GastoRecurrente.Precio = model.Importe;
                _GastoRecurrente.idTipoPago = int.Parse(Request.Form["TiposPagoEdit"].ToString());

                //si és el mateix tipus de gasto, no recalculem la nova data.
                if (int.Parse(Request.Form["TipoPeriodicidadEdit"].ToString()) != _GastoRecurrente.Periocidad)
                {
                    _GastoRecurrente.Periocidad = int.Parse(Request.Form["TipoPeriodicidadEdit"].ToString());
                    _GastoRecurrente.SiguienteEjecucion = _getSiguienteEjecucion(_GastoRecurrente.Periocidad);
                }

                _GastoRecurrente.GastoComputable = (model.GastoComputable == true);
                _GastoRecurrente.Activo = (model.GastoActivo ? 1 : 0);

                context.Entry(_GastoRecurrente).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();


                ModelState.Clear();
                return RedirectToAction("GastosRecurrentes", "Gastos");
            }
            return PartialView("_GastoRecurrente", model);
        }

        /*
        private void ModificarAcumulatsPosteriors(Gasto _gasto)
        {
            //var _GastoAcumulado = new SummaryGastoAcumuladoPorDia();
            var _GastoDiaCanvi = new SummaryGastoAcumuladoPorDia();
            List<SummaryGastoAcumuladoPorDia> _GastosAModificar;
            var context = new ExpensesEF.Entities();
            
            _GastosAModificar = context.SummaryGastoAcumuladoPorDia.Where(x=>x.Fecha >= _gasto.Fecha && x.idUser==_gasto.idUserGasto ).ToList();

            foreach (var _GastoAcumulado in _GastosAModificar)
            {
                _GastoAcumulado.GastoAcumuladoAnual -= _gasto.Precio;
                if (_GastoAcumulado.Fecha == _gasto.Fecha)
                {
                    _GastoAcumulado.ItemsComprados -= 1;
                }
                context.Entry(_GastoAcumulado).State = System.Data.Entity.EntityState.Modified;
            }
            context.SaveChanges();
        }
        */


        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            
            var _Gasto = new Gasto();
            

            using (var context = new ExpensesEF.Entities())
            {
                _Gasto = context.Gasto.Find(id);
                //Actualitzem gastos acumulats...
                if (_Gasto != null) { context.spAddSubPrecioGastosAcumulados(_Gasto.idGasto, 0, 0); }

                //Eliminem Gasto
                context.Entry(_Gasto).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }

            return RedirectToAction("IndexA", "Home");
        }
        
        [Authorize]
        public ActionResult DelRecurrente()
        {
            int _idGastoRecurrente = int.Parse(Request.Form["hidden-idd"]);
            var _GastoRecurrente = new GastoRecurrente();

            using (var context = new ExpensesEF.Entities())
            {
                _GastoRecurrente = context.GastoRecurrente.Find(_idGastoRecurrente);
                context.Entry(_GastoRecurrente).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }

            return RedirectToAction("GastosRecurrentes", "Gastos");
        }


        [Authorize]
        [HttpGet]
        public ActionResult GastosRecurrentes()
        {

            //return PartialView("GastosRecurrentes");
            GastosRecurrenteViewModels model = new GastosRecurrenteViewModels();
            var context = new ExpensesEF.Entities();
            string user;

            user= context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();

            model.ImportePeriodicoMensual = context.GastoRecurrente.Where(x => x.idUserGastoRecurrente == user && x.Periocidad == 1).Sum(x => x.Precio); 

            return View("GastosRecurrentes",model);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult AddGastoRecurrente(GastosRecurrenteViewModels model)
        {
            model.Importe = expenses.Utils.parseDecimal(Request.Form["txtImporte"].ToString());
            int subType;
            if (ModelState.IsValid)
            {
                // Save it in database
                var context = new ExpensesEF.Entities();
                //_codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);

                GastoRecurrente _GastoRecurrente = new GastoRecurrente();
                _GastoRecurrente.idUserGastoRecurrente = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
                _GastoRecurrente.idTipoGastoRecurrente = int.Parse(Request.Form["TiposGasto"].ToString());
                _GastoRecurrente.Concepto = model.Concepto;
                _GastoRecurrente.Precio = model.Importe;
                _GastoRecurrente.idTipoPago = int.Parse(Request.Form["TiposPago"].ToString());
                _GastoRecurrente.Periocidad = int.Parse(Request.Form["TipoPeriodicidad"].ToString());
                _GastoRecurrente.SiguienteEjecucion = _getSiguienteEjecucion(_GastoRecurrente.Periocidad);
                _GastoRecurrente.GastoComputable = (model.GastoComputable == true);
                _GastoRecurrente.Activo = (model.GastoActivo?1:0);

                subType= (Request.Form["IdSubType"] != "" ? int.Parse(Request.Form["IdSubType"]) : -1);

                if (subType>0)
                    {
                    _GastoRecurrente.idSubTipoGasto = subType;
                    }

                context.GastoRecurrente.Add(_GastoRecurrente);
                context.SaveChanges();


                ModelState.Clear();
                return RedirectToAction("GastosRecurrentes", "Gastos");
            }
            return PartialView("_GastoRecurrente", model);
        }


        #endregion


        #region "Private methods"

        
        private DateTime _getSiguienteEjecucion(int idTipoPeriodicidad)
        {
            var context = new ExpensesEF.Entities();
            int _mesesASumar;
            _mesesASumar = (context.Periocidad.Where(x => x.idPeriocidad == idTipoPeriodicidad).FirstOrDefault().MesesASumar).GetValueOrDefault();
            return DateTime.Now.AddMonths(_mesesASumar);

        }

        [Authorize]
        private ActionResult UpdateGasto(GastosViewModels model, int id)
        {
            decimal ValorOriginal=0;
            var _Gasto = new Gasto();
            string _codigo;
            var context = new ExpensesEF.Entities();
            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);

            if (!ModelState.IsValid)
            {
                model._TipoPagoList = context.TipoPagoTextosTraduccion.Where(x => (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).ToList();
                model._TipoGastosTrad = GetListTipoGastos(context.TipoGastoTextosTraduccion.Where(x => (x.TipoGasto.Activo == true) && (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).ToList().OrderBy(x => x.Descripcion));
                model._GrupoGastosTrad = GetListItemsGrupoGastos(context.GrupoGastoTextosTraduccion.Where(x => (x.GrupoGasto.Activo == true) && (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).ToList().OrderBy(x => x.Descripcion));
                model._FormatoFecha = GetFormatoFecha();
                return View(model);
            }
            
            _Gasto = context.Gasto.Where(x => x.idGasto == id).FirstOrDefault();
            ValorOriginal = _Gasto.Precio;
            _Gasto.idGasto = id;
            _Gasto.Concepto = model.Concepto;
            _Gasto.Fecha = model.Fecha;
            _Gasto.Precio = model.Importe;
            _Gasto.Valoracion = model.Valoracion;
            _Gasto.idUserGasto = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
            //_Gasto.idTipoGasto = model._idSelectedGasto;
            _Gasto.idTipoPago = model._idSelectedTipoPagoGasto;

            _Gasto.EsRegalo = model.EsRegalo;

            /*
            if (model._idSelectedGrupoGasto > -1)
            { _Gasto.idGrupoGasto = model._idSelectedGrupoGasto; }
            else
            { _Gasto.idGrupoGasto = null; }
            */

            context.Entry(_Gasto).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            //Updatamos el total acumulado.
            if (ValorOriginal> model.Importe)
            {
                context.spAddSubPrecioGastosAcumulados(_Gasto.idGasto,2,ValorOriginal - model.Importe);
            }
            else
            {
                context.spAddSubPrecioGastosAcumulados(_Gasto.idGasto, 3, model.Importe - ValorOriginal);
            }


            return RedirectToAction("IndexA", "Home");
    }

        private ActionResult AddGasto(GastosViewModels model, int idSubType)
        {
            var context = new ExpensesEF.Entities();
            string _codigo;
            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);


            if (!ModelState.IsValid)
            {
                model._TipoGastosTrad = GetListTipoGastos(context.TipoGastoTextosTraduccion.Where(x => (x.TipoGasto.Activo == true) && (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).ToList().OrderBy(x => x.Descripcion));
                model._GrupoGastosTrad = GetListItemsGrupoGastos(context.GrupoGastoTextosTraduccion.Where(x => (x.GrupoGasto.Activo == true) && (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).ToList().OrderBy(x => x.Descripcion));
                model._TipoPagoList = context.TipoPagoTextosTraduccion.Where(x => (x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma)).ToList();
                model._PersonasACompartir = GetListItemsPersonas(context.AspNetUsers.Where(y => y.Email != System.Web.HttpContext.Current.User.Identity.Name).OrderBy(y => y.Email));
                model._FormatoFecha = GetFormatoFecha();
                return View(model);
            }

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                Gasto _Gasto = new Gasto();
                Gasto _GastoCopiado = new Gasto();
                _Gasto.Concepto = model.Concepto;
                _Gasto.Fecha = model.Fecha;
                _Gasto.idTipoGasto = model._idSelectedGasto;
                _Gasto.GastoRecurrente = false;
                _Gasto.Resaltar = false;
                _Gasto.EsRegalo = model.EsRegalo;
                _Gasto.GastoComputable = 1;
                _Gasto.GastoEditable = true;
                _Gasto.Valoracion = model.Valoracion;
                if (idSubType > 0)
                    { _Gasto.idSubTipoGasto = idSubType; }

                if (model._idSelectedGrupoGasto > -1)
                { _Gasto.idGrupoGasto = model._idSelectedGrupoGasto; }

                _Gasto.idUserGasto = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
                _Gasto.idTipoPago = model._idSelectedTipoPagoGasto;

                //Si el gasto es compartido...
                if (model.EsCompartido)
                {
                    _Gasto.GastoEditable = false;
                    _Gasto.Precio = model.Importe/2;
                    _Gasto.EsCompartido = true;
                    _GastoCopiado = CopiarGasto(_Gasto, model._idSelectedPersonaConQuienComparte);

                    context.Gasto.Add(_GastoCopiado);
                    context.SaveChanges();

                    context.Gasto.Add(_Gasto);
                    context.SaveChanges();

                    GastoCompartido _GastoCompartido = new GastoCompartido();
                    _GastoCompartido.idGasto1 = _Gasto.idGasto;
                    _GastoCompartido.idGasto2 = _GastoCopiado.idGasto;
                    _GastoCompartido.fecha = model.Fecha;
                    _GastoCompartido.precio = model.Importe;
                    context.GastoCompartido.Add(_GastoCompartido);
                    context.SaveChanges();

                    context.spAddSubPrecioGastosAcumulados(_GastoCopiado.idGasto, 1, 0);
                    context.spAddSubPrecioGastosAcumulados(_Gasto.idGasto, 1, 0);

                }
                else
                {
                    _Gasto.EsCompartido = false;
                    _Gasto.Precio = model.Importe;

                    context.Gasto.Add(_Gasto);
                    context.SaveChanges();

                    //Actualitzem gastos acumulats
                    context.spAddSubPrecioGastosAcumulados(_Gasto.idGasto, 1,0);
                }
                dbContextTransaction.Commit();
            }

            //model._TipoGastosTrad = GetListTipoGastos(context.TipoGastoTextosTraduccion.Where(x => x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma).ToList());
            return RedirectToAction("IndexA", "Home");
        }

        #endregion

        #region "Cargas Combos"


        public ActionResult GetDefaultValueSubTipo(int id)
        {
            string _res;
            string _User;
            string _codigo;
            var context = new ExpensesEF.Entities();

            try
            {
                _User = context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id.ToString();
                _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);
                _res = context.ValuesDefaultConcepto.Where(x => x.idUser == _User && x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma && x.idSubTipoGasto == id).FirstOrDefault().DefaultConcepto.ToString();
            }
            catch (Exception _ex)
            { _res = ""; }
            
            return Json(_res);
        }


        public ActionResult GetSubTiposTipo(int id)
        {
            var context = new ExpensesEF.Entities();
            IEnumerable<SelectListItem> lista;
            string _codigo;
            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);


            lista = GetSubTiposGastos(context.SubTipoGastoTextosTraduccion.Where(x => x.SubTipoGasto.idTipoGasto==id && x.SubTipoGasto.Activo==true &&  x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma).OrderBy(x =>x.Descripcion));

            return Json(lista.Select(x => new
            {
                idSubTipo = x.Value,
                NombreSubTipo = x.Text
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

       

        public ActionResult getTiposPago()
        {
            var context = new ExpensesEF.Entities();
            IEnumerable<SelectListItem> lista;
            string _codigo;
            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);

            lista = GetListTipoPagos(context.TipoPagoTextosTraduccion.Where(x => x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma).OrderBy(x => x.Descripcion));

            return Json(lista.Select(x => new
            {
                idTipoPago = x.Value,
                NombreTipoPago = x.Text
            }).ToList(), JsonRequestBehavior.AllowGet);
        }



        private Gasto CopiarGasto(Gasto _Gasto, string _PersonaConQuienComparte)
        {
            Gasto _GastoCopiado = new Gasto();

            _GastoCopiado.Concepto = _Gasto.Concepto;
            _GastoCopiado.Fecha = _Gasto.Fecha;
            _GastoCopiado.idTipoGasto = _Gasto.idTipoGasto;
            _GastoCopiado.GastoRecurrente = false;
            _GastoCopiado.Resaltar = false;
            _GastoCopiado.EsRegalo = _Gasto.EsRegalo;
            _GastoCopiado.GastoComputable = 1;
            _GastoCopiado.GastoEditable = false;
            _GastoCopiado.idSubTipoGasto = _Gasto.idSubTipoGasto; 
            _GastoCopiado.idGrupoGasto = _Gasto.idGrupoGasto;
            _GastoCopiado.idUserGasto = _PersonaConQuienComparte;
            _GastoCopiado.idTipoPago = _Gasto.idTipoPago;
            _GastoCopiado.Precio = _Gasto.Precio;
            _GastoCopiado.EsCompartido = _Gasto.EsCompartido;
            _GastoCopiado.Valoracion = _Gasto.Valoracion;
            
            return _GastoCopiado;
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


        private IEnumerable<SelectListItem> GetListTipoPagos(IEnumerable<TipoPagoTextosTraduccion> elements)
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
                    Value = element.idTipoPago.ToString(),
                    Text = element.Descripcion
                });
            }

            return selectList;
        }


        public ActionResult getTipoPeriocidad()
        {

            var context = new ExpensesEF.Entities();
            IEnumerable<SelectListItem> lista;
            string _codigo;
            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);


            lista = GetListTipoPeriodicidad(context.PeriocidadTextosTraduccion.Where(x => x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma).OrderBy(x=>x.Descripcion));

            return Json(lista.Select(x => new
            {
                idPeriodicidad = x.Value,
                NombrePeriodicidad = x.Text
            }).ToList(), JsonRequestBehavior.AllowGet);

        }

       
        public ActionResult getTiposGasto()
        {

            var context = new ExpensesEF.Entities();
            IEnumerable<SelectListItem> lista;
            string _codigo;
            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);

            lista = GetListTipoGastos(context.TipoGastoTextosTraduccion.Where(x => x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma).OrderBy(x => x.Descripcion));

            return Json(lista.Select(x => new
            {
                idTipoGesto = x.Value,
                NombreTipoGasto = x.Text
            }).ToList(), JsonRequestBehavior.AllowGet);

        }

        public ActionResult getSubTiposTipoFirst()
        {
            var context = new ExpensesEF.Entities();
            int id;
            string _codigo;
            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);

            //Obtenim l'id del primer subtipus ordenat alfabèticament (és a dir, el mateix que es veurà a la CBO)
            id = context.TipoGastoTextosTraduccion.Where(x => x.TipoGasto.Activo == true && x.idIdioma == context.Idiomas.Where(y => y.codigo == _codigo).FirstOrDefault().idIdioma).OrderBy(x => x.Descripcion).FirstOrDefault().idTipoGasto;

            return GetSubTiposTipo(id);
           
        }





        private string GetFormatoFecha()
        {
            string _codigo;
            _codigo = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);
            

            switch (_codigo)
            {
                case "es":
                    return "{0:dd/MM/yyyy}";

                default:
                    return "{0:MM/dd/yyyy}";
            }
        }

        private IEnumerable<SelectListItem> GetListTipoPeriodicidad(IEnumerable<PeriocidadTextosTraduccion> elements)
        {
            var selectList = new List<SelectListItem>();

            // For each string in the 'elements' variable, create a new SelectListItem object
            // that has both its Value and Text properties set to a particular value.
            // This will result in MVC rendering each item as:
            //     <option value="State Name">State Name</option>
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.idPeriocidad.ToString(),
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

        private IEnumerable<SelectListItem> GetListItemsPersonas(IEnumerable<AspNetUsers> elements)
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
                    Value = element.Id.ToString(),
                    Text = element.Email
                });
            }
            return selectList;
        }

        private IEnumerable<SelectListItem> GetListItemsGrupoGastos(IEnumerable<GrupoGastoTextosTraduccion> elements)
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

            selectList.Insert(0, new SelectListItem { Text = "", Value = "-1" });

            return selectList;
        }
        #endregion

    }
}
