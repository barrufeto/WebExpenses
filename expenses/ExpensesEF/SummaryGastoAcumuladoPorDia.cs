//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpensesEF
{
    using System;
    using System.Collections.Generic;
    
    public partial class SummaryGastoAcumuladoPorDia
    {
        public string idUser { get; set; }
        public System.DateTime Fecha { get; set; }
        public int DayInYear { get; set; }
        public int ItemsComprados { get; set; }
        public decimal GastoAcumuladoAnual { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
