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
    
    public partial class PeriocidadTextosTraduccion
    {
        public int idPeriocidad { get; set; }
        public int idIdioma { get; set; }
        public string Descripcion { get; set; }
    
        public virtual Idiomas Idiomas { get; set; }
        public virtual Periocidad Periocidad { get; set; }
    }
}
