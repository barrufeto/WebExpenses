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
    
    public partial class SummaryPorSubTipoGasto
    {
        public string idUser { get; set; }
        public int idSubTipoGasto { get; set; }
        public int Año { get; set; }
        public Nullable<decimal> Valor { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual SubTipoGasto SubTipoGasto { get; set; }
    }
}
