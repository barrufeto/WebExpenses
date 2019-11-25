using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using Microsoft.AspNet.Identity;
using ExpensesEF;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using experses;

namespace experses.Models
{

    public class GastosViewModels
    {


        //public Gasto _Gasto { get; set; }
        public string idUserGasto { get; set; }

        [Required]
        public decimal Importe { get; set; }

        [Required(ErrorMessage = "Concepto is Required.")]
        public string Concepto { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "aaaaa")]
        public DateTime Fecha { get; set; }

        public string _FormatoFecha { get; set; }

        [Required]
        public int idTipoGasto { get; set; }
        
        //Tipus de pagament
        public int _idTipoPago { get; set; }
        public int _idSelectedTipoPagoGasto { get; set; }
        public List<TipoPagoTextosTraduccion> _TipoPagoList { get; set; }
        public int _idDefaultTipoPagoGasto { get; set; }


        //Grup de gastos
        public int idGrupoGasto { get; set; }
        public int _idSelectedGrupoGasto { get; set; }
        public IEnumerable<SelectListItem> _GrupoGastosTrad { get; set; }
        public string _DescripcionGrupoGasto { get; set; }

        //Tipus de gasto
        public int _idSelectedGasto { get; set; }
        public IEnumerable<SelectListItem> _TipoGastosTrad { get; set; }
        public string _DescripcionTipoGasto { get; set; }

        //SubTipus de gasto
        public int _idSelectedSubTipoGasto { get; set; }
        public IEnumerable<SelectListItem> _SubTipoGastosTrad { get; set; }
        public string _DescripcionSubTipoGasto { get; set; }

        
        public bool EsRegalo { get; set; }

        public bool EsCompartido { get; set; }

        //SubTipus de gasto
        public string _idSelectedPersonaConQuienComparte { get; set; }
        public IEnumerable<SelectListItem> _PersonasACompartir { get; set; }
        //public string _DescripcionSubTipoGasto { get; set; }

        public int Valoracion { get; set; }
    }


    public class GastosRecurrenteViewModels
    {


        //public Gasto _Gasto { get; set; }
        public string idUserGasto { get; set; }

        [Required]
        public decimal Importe { get; set; }

        [Required(ErrorMessage = "Concepto is Required.")]
        public string Concepto { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Fecha { get; set; }

        public string _FormatoFecha { get; set; }

        [Required]
        public int idTipoGasto { get; set; }

        //Tipus de gasto
        public int _idSelectedGasto { get; set; }
        public IEnumerable<SelectListItem> _TipoGastosTrad { get; set; }

        //Tipus de pagament
        public int _idTipoPago { get; set; }
        public int _idSelectedTipoPagoGasto { get; set; }
        public List<TipoPagoTextosTraduccion> _TipoPagoList { get; set; }

        //Si és mensual, anual....
        public int _idTipoRecurrencia { get; set; }

        [Required]
        public bool GastoActivo { get; set; }

        [Required]
        public bool GastoComputable { get; set; }

        public GastosRecurrenteViewModels()
          {
            Concepto = "aa";
          }


        public decimal ImportePeriodicoMensual { get; set; }

    }



    public class InOutViewUserModel
    {
        public vst_InOut _InOuts { get; set; }
    }


    

}