using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;

namespace experses.Models
{
    public class ChartGastoMensual
    {
       
        public decimal GastoMesActual { get; set; }
        public decimal GastoMesAnterior { get; set; }


    }
}