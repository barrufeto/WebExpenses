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
    
    public partial class SummaryTotal_Mensual
    {
        public string idUser { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }
        public decimal GastoTotal { get; set; }
        public decimal GastoSoloComputable { get; set; }
        public decimal GastoNoComputable { get; set; }
        public decimal PrevisionGasto { get; set; }
        public Nullable<decimal> AhorroFinDeMes { get; set; }
    }
}