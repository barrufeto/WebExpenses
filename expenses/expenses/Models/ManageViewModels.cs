using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.Mvc;

namespace expenses.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }

        public bool VerTpcGastovsMesAnterior { get; set; }

        public bool VerGastosTodosOSinRecurrentes { get; set; }

        public string idUserGasto { get; set; }
        public string DatosSueldos { get; set; }

        public int _idSelectedGrupoGasto { get; set; }
        public IEnumerable<SelectListItem> _GrupoGastosTrad { get; set; }

        public int _idSelectedTipoPago { get; set; }
        public IEnumerable<SelectListItem> _TiposPagoTrad { get; set; }


        /*
          
        
        
        public decimal? SueldoBrutoAnual { get; set; }
        public decimal? SueldoNetoEnero { get; set; }
        public decimal? SueldoNetoFebrero { get; set; }
        public decimal? SueldoNetoMarzo { get; set; }
        public decimal? SueldoNetoAbril { get; set; }
        public decimal? SueldoNetoMayo { get; set; }
        public decimal? SueldoNetoJunio { get; set; }
        public decimal? SueldoNetoJulio { get; set; }
        public decimal? SueldoNetoAgosto { get; set; }
        public decimal? SueldoNetoSeptiembre { get; set; }
        public decimal? SueldoNetoOctubre { get; set; }
        public decimal? SueldoNetoNoviembre { get; set; }
        public decimal? SueldoNetoDiciembre { get; set; }
        */

    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña nueva")]
        public string NewPassword { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirme la contraseña nueva")]
        //[Compare("NewPassword", ErrorMessage = "La contraseña nueva y la contraseña de confirmación no coinciden.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirme la contraseña nueva")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword",
                    ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña nueva")]
        public string NewPassword { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirme la contraseña nueva")]
        //[Compare("NewPassword", ErrorMessage = "La contraseña nueva y la contraseña de confirmación no coinciden.")]

        [DataType(DataType.Password)]
        [Display(Name = "Confirme la contraseña nueva")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword",
                    ErrorMessage = "The password and confirmation password do not match.")]

        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Número de teléfono")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Número de teléfono")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}