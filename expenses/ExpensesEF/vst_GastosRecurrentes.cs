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
    
    public partial class vst_GastosRecurrentes
    {
        public int idGastoRecurrente { get; set; }
        public string Concepto { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public bool GastoComputable { get; set; }
        public Nullable<int> Activo { get; set; }
        public System.DateTime SiguienteEjecucion { get; set; }
        public int idIdioma { get; set; }
        public string idUserGastoRecurrente { get; set; }
        public string TextoActivo { get; set; }
        public string TextoComputable { get; set; }
        public int idtipoGastoRecurrente { get; set; }
        public int idTipoPago { get; set; }
        public int Periocidad { get; set; }
        public Nullable<int> IdIdiomaSubTipo { get; set; }
        public string Descripcion_SubTipo { get; set; }
    }
}