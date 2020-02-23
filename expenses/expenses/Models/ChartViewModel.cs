using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;
using System.Web.Mvc;

namespace experses.Models
{
    public class ChartGastoMensual
    {
       
        public decimal GastoMesActual { get; set; }
        public decimal GastoMesAnterior { get; set; }
    }

    public class RptTipoModel
    {
        //SubTipus de gasto
        public int _idSelectedTipoGasto { get; set; }
        public IEnumerable<SelectListItem> _TipoGastosTrad { get; set; }

        //Grupos de gasto
        public int _idSelectedGrupoGasto { get; set; }
        public IEnumerable<SelectListItem> _GrupoGastosTrad { get; set; }



    }
}