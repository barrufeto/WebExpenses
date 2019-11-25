using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;

namespace experses.Models
{
    public class ChartViewModel
    {


        public enum eTipoChart
        {
            MesActual=0,
            AnyoActual,
            Otro
        }

        public enum eEstiloChart
        {
              Barras = 0,
              Quesito
        }

        public Chart Chart { get; set; }
        public eTipoChart _TipoChart { get; set; }
        public eEstiloChart _EstiloChart { get; set; }

        public DateTime _DateFrom { get; set; }
        public DateTime _DateTo { get; set; }



    }
}