[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(expenses.MVCGridConfig), "RegisterGrids")]

namespace expenses
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;
    using expenses;
    using MVCGrid.Models;
    using MVCGrid.Web;
    using ExpensesEF;
    using System.Linq.Expressions;
    using System.Text;

    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {

            MVCGridDefinitionTable.Add("InOutUserGrid", new MVCGridBuilder<vst_InOut>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)

               .AddColumns(cols =>
               {
               // Add your columns here
               cols.Add("Fecha").WithColumnName(Resources.Descripciones.GridGastoFecha)
                  .WithFiltering(false)
                   .WithHeaderText(" ")
                  //.WithHeaderText(Resources.Descripciones.GridGastoFecha)
                  .WithCellCssClassExpression(p => "col-sm-1")
                  //.WithValueExpression(i => i.Fecha.HasValue ? i.Fecha.Value.ToShortDateString() : "");
                  .WithValueExpression(i => i.Fecha.ToShortDateString());

               cols.Add("TipoGasto") //.WithColumnName(Resources.Descripciones.GridTipoGasto)
                  .WithFiltering(true)
                   .WithHeaderText(" ")
                  .WithCellCssClassExpression(p => "col-sm-2")
                  //.WithHeaderText(Resources.Descripciones.GridTipoGasto)
                  .WithValueExpression(i => i.Descripcion); // use the Value Expression to return the cell text for this column

                   cols.Add("SubTipoGasto") //.WithColumnName(Resources.Descripciones.GridTipoGasto)
                    .WithFiltering(true)
                     .WithHeaderText(" ")
                    .WithCellCssClassExpression(p => "col-sm-2")
                    //.WithHeaderText(Resources.Descripciones.GridTipoGasto)
                    .WithValueExpression(i => i.Descripcion_Subtipo); // use the Value Expression to return the cell text for this column


                   cols.Add("Concepto").WithColumnName(Resources.Descripciones.GridGastoConcepto)
                   .WithCellCssClassExpression(p => "col-sm-4")
                    .WithHeaderText(" ")
                  //.WithHeaderText(Resources.Descripciones.GridGastoConcepto)
                  .WithValueExpression(i => i.Concepto) // use the Value Expression to return the cell text for this column
                  .WithFiltering(true);


               cols.Add("Precio").WithColumnName(Resources.Descripciones.GridGastoImporte)
                  .WithFiltering(true)
                  .WithHeaderText(" ")
                  .WithCellCssClassExpression(p => "col-sm-1")
                  //.WithHeaderText(Resources.Descripciones.GridGastoImporte)
                  .WithValueExpression(i => i.Precio.ToString()); // use the Value Expression to return the cell text for this column

                   /*
               cols.Add("Delete").WithHtmlEncoding(false)
                  .WithFiltering(false)
                  .WithSorting(false)
                  .WithHeaderText(" ")
                  .WithCellCssClassExpression(p => "col-sm-2")
                  .WithValueExpression(i => i.idGasto.ToString())
                   .WithValueTemplate("<a href = '/Gastos/Edit/{value}'><img src='/Img/iconos/edit.png'></a>"
                   + " <a href = '/Gastos/Del/{value}'><img src='/Img/iconos/delete24.png'</a>"
                   + " <a href = '/Gastos/Copy/{value}'><img src='/Img/iconos/copy.png'</a>"
              );
              */
               cols.Add("Butons").WithHtmlEncoding(false)
                   .WithFiltering(false)
                  .WithSorting(false)
                  .WithHeaderText(" ")
                  .WithCellCssClassExpression(p => "col-sm-2")
                  .WithValueExpression(i => i.idGasto.ToString())
                  .WithValueExpression((p, c) =>
                   {

                       StringBuilder sb = new StringBuilder();

                       
                       if (bool.Parse(p.GastoEditable.GetValueOrDefault().ToString()))
                       {
                           string Action = c.UrlHelper.Action("Edit", "Gastos", new { id = p.idGasto });
                           string href = " href= " + Action + " class=approve >";
                           sb.Append(" <a  ");
                           //  sb.Append(target);
                           sb.Append(href);
                           sb.Append("<img src = '/Img/iconos/edit.png' >");
                           sb.Append("</a> ");

                           Action = c.UrlHelper.Action("Delete", "Gastos", new { id = p.idGasto });
                           href = " href= " + Action + " class=approve >";
                           sb.Append(" <a  ");
                           //  sb.Append(target);
                           sb.Append(href);
                           sb.Append("<img src = '/Img/iconos/Delete24.png' >");
                           sb.Append("</a> ");


                           //sb.Append("<img src = \"/Img/iconos/Delete24.png\" style=\"cursor: hand; cursor: pointer\" name = \"image\" data-toggle = \"modal\" data-target = \"#myModalDelGasto\" data-id = \"" + p.idGasto + "\" data-datos = \"" + p.Concepto + "\" />");


                           if (p.GastoRecurrente == false)
                           {
                               Action = c.UrlHelper.Action("Copy", "Gastos", new { id = p.idGasto });
                               href = " href= " + Action + " class=approve >";
                               sb.Append(" <a  ");
                               sb.Append(href);
                               sb.Append("<img src = '/Img/iconos/copy.png' >");
                               sb.Append("</a> ");
                           }

                           //sb.Append("<img src = \"/Img/iconos/ver.png\" style=\"cursor: hand; cursor: pointer\" name = \"image\" data-toggle = \"modal\" data-target = \"#myModalVerGasto\" data-id = \"" + p.idGasto + "\" data-datos = \"" + p.Concepto + "|" + p.Descripcion + "|" + p.Precio + "|" + p.Fecha + "|" + p.GrupoGasto + "|" + p.GastoComputable + "\" />");

                       }
                       else
                       {
                           //sb.Append("<img src = \"/Img/iconos/ver.png\" style=\"cursor: hand; cursor: pointer\" name = \"image\" data-toggle = \"modal\" data-target = \"#myModalVerGasto\" data-id = \"" + p.idGasto + "\" data-datos = \"" + p.Concepto + "|" + p.Descripcion + "|" + p.Precio + "|" + p.Fecha + "|" + p.GrupoGasto + "|" + p.GastoComputable + "\" />");
                           string Action = c.UrlHelper.Action("Ver", "Gastos", new { id = p.idGasto });
                           string href = " href= " + Action + " class=approve >";
                           sb.Append(" <a  ");
                           //sb.Append(target);
                           sb.Append(href);
                           sb.Append("<img src = '/Img/iconos/ver.png' >");
                           sb.Append("</a> ");

                           Action = c.UrlHelper.Action("Copy", "Gastos", new { id = p.idGasto });
                           href = " href= " + Action + " class=approve >";
                           sb.Append(" <a  ");
                           sb.Append(href);
                           sb.Append("<img src = '/Img/iconos/copy.png' >");
                           sb.Append("</a> ");
                       }
                       
                       
                       return sb.ToString();
                   });


               })
               .WithPageParameterNames("user")
               .WithPaging(true, 10)
               //.WithSorting(true, "Concepto", SortDirection.Dsc)
               .WithRowCssClassExpression(x => (x.GastoRecurrente == true) ? "info" : (x.Precio > 300) ? "danger" : "")
               .WithSummaryMessage(" {0} - {1}  ({2})")
               .WithFiltering(true)
               .WithProcessingMessage("....")
               .WithNextButtonCaption("")
               .WithPreviousButtonCaption("")
              .WithRetrieveDataMethod((options) =>
              {

                  string _pEmail = options.QueryOptions.GetPageParameterString("user");
                  string _pIdioma = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);

                  var result = new QueryResult<vst_InOut>();
                  using (var context = new ExpensesEF.Entities())
                  {
                      int _idioma = context.Idiomas.Where(x => x.codigo == _pIdioma).FirstOrDefault().idIdioma;
                      //string _user = context.AspNetUsers.Where(x => x.Email == _pEmail).FirstOrDefault().Id.ToString();

                      AspNetUsers _usuario = context.AspNetUsers.Where(x => x.Email == _pEmail).FirstOrDefault();
                      string _user = _usuario.Id.ToString();
                      bool _GastoSinRecurrentes = _usuario.VerGastosSinOConRecurrentes.GetValueOrDefault();


                      string _concepto = "";

                      if (options.QueryOptions.Filters.Count > 0)
                      {
                          _concepto = ((options.QueryOptions.GetFilterString("Concepto") == null) ? "" : options.QueryOptions.GetFilterString("Concepto").ToString());
                          
                      }
                      System.Linq.IQueryable<vst_InOut>  query;

                      if (_GastoSinRecurrentes)
                      {
                          //query = (context.vst_InOut.Where(x => x.idIdioma == _idioma && (x.IdIdiomaSubTipo == _idioma || x.IdIdiomaSubTipo == null) && x.idUserGasto == _user && (x.Concepto.Contains(_concepto) && x.Descripcion.Contains(_TipoGasto))).OrderByDescending(x => x.Fecha).ThenByDescending(x => x.idGasto).AsQueryable());
                          query = (context.vst_InOut.Where(x => x.idIdioma == _idioma && (x.IdIdiomaSubTipo == _idioma || x.IdIdiomaSubTipo == null) && x.idUserGasto == _user && (x.Concepto.Contains(_concepto) || x.Descripcion.Contains(_concepto) || x.Descripcion_Subtipo.Contains(_concepto) )).OrderByDescending(x => x.Fecha).ThenByDescending(x => x.idGasto).AsQueryable());
                      }
                      else
                      {
                          //query = (context.vst_InOut.Where(x =>  x.GastoRecurrente == false &&  x.idIdioma == _idioma && (x.IdIdiomaSubTipo == _idioma || x.IdIdiomaSubTipo == null) && x.idUserGasto == _user && (x.Concepto.Contains(_concepto) && x.Descripcion.Contains(_TipoGasto))).OrderByDescending(x => x.Fecha).ThenByDescending(x => x.idGasto).AsQueryable());
                          query = (context.vst_InOut.Where(x => x.GastoRecurrente == false && x.idIdioma == _idioma && (x.IdIdiomaSubTipo == _idioma || x.IdIdiomaSubTipo == null) && x.idUserGasto == _user && (x.Concepto.Contains(_concepto) || x.Descripcion.Contains(_concepto) || x.Descripcion_Subtipo.Contains(_concepto))).OrderByDescending(x => x.Fecha).ThenByDescending(x => x.idGasto).AsQueryable());
                      }
                      
                      

                      result.TotalRecords = query.Count(); 


                       if (options.QueryOptions.GetLimitOffset().HasValue)
                      {
                          query = query.Skip(options.QueryOptions.GetLimitOffset().Value).Take(options.QueryOptions.GetLimitRowcount().Value);
                      }
                       result.Items = query.ToList();

                  }
                  return result;
               }
              )
                //.WithRowCssClassExpression(new ExpensesEF.Entities().AspNetUsers.Where(x=>) ? "info" : (x.Precio > 300) ? "danger" : "")
                
               );


            MVCGridDefinitionTable.Add("GastosRecurrentesGrid", new MVCGridBuilder<vst_GastosRecurrentes>()
            .WithAuthorizationType(AuthorizationType.AllowAnonymous)

            .AddColumns(cols =>
              {
                  cols.Add("Concepto").WithColumnName(Resources.Descripciones.GridGastoConcepto)
                      .WithFiltering(true)
                      .WithHeaderText(" ")
                      .WithCellCssClassExpression(p => "col-sm-3")
                      .WithValueExpression(i => i.Concepto.ToString()); 

                  cols.Add("Precio").WithColumnName(Resources.Descripciones.GridGastoImporte)
                      .WithFiltering(true)
                      .WithHeaderText(" ")
                      .WithCellCssClassExpression(p => "col-sm-1")
                      .WithValueExpression(i => i.Precio.ToString());

                  cols.Add("Periodicidad")
                     .WithFiltering(true)
                     .WithHeaderText(" ")
                     .WithCellCssClassExpression(p => "col-sm-1")
                     .WithValueExpression(i => i.Descripcion.ToString());

                  cols.Add("Activo")
                    .WithHeaderText(" ")
                    .WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression(i => i.TextoActivo.ToString());

                  cols.Add("Computable")
                    .WithHeaderText(" ")
                    .WithCellCssClassExpression(p => "col-sm-2")
                    .WithValueExpression(i => i.TextoComputable.ToString());

                  cols.Add("Fecha").WithColumnName(Resources.Descripciones.GridGastoFecha)
                     .WithFiltering(false)
                     .WithHeaderText(" ")
                     .WithCellCssClassExpression(p => "col-sm-1")
                     .WithValueExpression(i => i.SiguienteEjecucion.ToShortDateString());

                  cols.Add("Butons").WithHtmlEncoding(false)
                   .WithFiltering(false)
                  .WithSorting(false)
                  .WithHeaderText(" ")
                  .WithCellCssClassExpression(p => "col-sm-2")
                  .WithValueExpression(i => i.idGastoRecurrente.ToString())
                  .WithValueExpression((p, c) =>
                  {

                      StringBuilder sb = new StringBuilder();
                      /*
                      string Action = c.UrlHelper.Action("EditRecurrente", "Gastos", new { id = p.idGastoRecurrente });
                      string href = " href= " + Action + " class=approve >";
                      sb.Append(" <a  ");
                      //  sb.Append(target);
                      sb.Append(href);
                      sb.Append("<img src = '/Img/iconos/edit.png' >");
                      sb.Append("</a> ");

                        
                      Action = c.UrlHelper.Action("DelRecurrente", "Gastos", new { id = p.idGastoRecurrente });
                      href = " href= " + Action + " class=approve >";
                      sb.Append(" <a  ");

                      sb.Append(href);
                      sb.Append("<img src = '/Img/iconos/delete24.png' >");
                      sb.Append("</a> ");
                      */
                      //class=\"btn btn-primary btn-lg\"

                      //"<img src = '/Img/iconos/delete24.png' >" + "
                      //string _text = "<button type = \"button\"  data-toggle=\"modal\" data-target=\"#myModal\" data-myvalue=\"" + p.idGastoRecurrente + "\">" + "dassd"  +  "</button>";

                      //string _text = "<button type = \"button\"   data-toggle=\"modal\" data-target=\"#myModal\" data-myvalue=\"" + p.idGastoRecurrente + "\" data-myvalue2=\"" + "ver" + "\" >" + "edit" + "</button>";


                      string _text = "<input type = \"image\" src = \"/Img/iconos/edit.png\" name = \"image\" data-toggle = \"modal\" data-target = \"#myModalEdit\" data-id = \"" + p.idGastoRecurrente + "\" data-datos = \"" + p.Concepto + "|" + p.idTipoPago + "|" + p.idtipoGastoRecurrente + "|" + p.Periocidad + "|" + p.Precio + "|" +p.GastoComputable + "|" + p.Activo + "|" + p.Descripcion_SubTipo  + "\" />";
                      _text += "<input type = \"image\" src = \"/Img/iconos/Delete24.png\" name = \"image\" data-toggle = \"modal\" data-target = \"#myModalDel\" data-id = \"" + p.idGastoRecurrente + "\" data-datos = \"" + p.Concepto + "\" />";
                      sb.Append(_text);

                      



                      return sb.ToString();
                  });

              })
               .WithPageParameterNames("user")
               .WithPaging(true, 20)
               .WithSorting(true, "Concepto", SortDirection.Dsc)
               .WithSummaryMessage(" {0} - {1}  ({2})")
               .WithFiltering(true)
               .WithProcessingMessage("....")
               .WithNextButtonCaption("")
               .WithPreviousButtonCaption("")

                .WithRetrieveDataMethod((options) =>
                {

                    string _pEmail = options.QueryOptions.GetPageParameterString("user");
                    string _pIdioma = System.Threading.Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);

                    var result = new QueryResult<vst_GastosRecurrentes>();

                    using (var context = new ExpensesEF.Entities())
                    {
                        int _idioma = context.Idiomas.Where(x => x.codigo == _pIdioma).FirstOrDefault().idIdioma;
                        string _user = context.AspNetUsers.Where(x => x.Email == _pEmail).FirstOrDefault().Id.ToString();

                        var query = (context.vst_GastosRecurrentes.Where(x => x.idIdioma == _idioma && (x.IdIdiomaSubTipo == _idioma || x.IdIdiomaSubTipo == null) && x.idUserGastoRecurrente == _user).OrderByDescending(x => x.SiguienteEjecucion).ThenByDescending(x => x.idGastoRecurrente).AsQueryable());

                        result.TotalRecords = query.Count();


                        if (options.QueryOptions.GetLimitOffset().HasValue)
                        {
                            query = query.Skip(options.QueryOptions.GetLimitOffset().Value).Take(options.QueryOptions.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                }
             )
            );
        }
    }
}