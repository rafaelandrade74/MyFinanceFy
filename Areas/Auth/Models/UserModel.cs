using MyFinanceFy.Libs.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyFinanceFy.Areas.Auth.Models
{
    public class UserModel
    {
        [Display(Name ="Email")]
        public string? Username { get; set; }
        [Display(Name = "Nome Completo")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Codigo obrigatorio!")]
        [Display(Name = "País")]
        public PhoneCountryCode PhoneCountryCode { get; set; }
        [Phone]
        [Display(Name = "Telefone")]
        public string? PhoneNumber { get; set; }
    }
}
