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
    
    public partial class GastoRecurrente
    {
        public int idGastoRecurrente { get; set; }
        public string idUserGastoRecurrente { get; set; }
        public int idTipoGastoRecurrente { get; set; }
        public string Concepto { get; set; }
        public decimal Precio { get; set; }
        public int idTipoPago { get; set; }
        public int Periocidad { get; set; }
        public System.DateTime SiguienteEjecucion { get; set; }
        public bool GastoComputable { get; set; }
        public Nullable<int> Activo { get; set; }
        public Nullable<int> idSubTipoGasto { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Periocidad Periocidad1 { get; set; }
        public virtual TipoGasto TipoGasto { get; set; }
        public virtual TipoPago TipoPago { get; set; }
    }
}
