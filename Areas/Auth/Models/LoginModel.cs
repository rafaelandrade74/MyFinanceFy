using System.ComponentModel.DataAnnotations;

namespace MyFinanceFy.Areas.Auth.Models
{
    public class LoginModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required(ErrorMessage = "Campo obrigatorio!")]
        [EmailAddress(ErrorMessage = "Email invalido!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required(ErrorMessage = "Campo obrigatorio!")]
        [StringLength(100, ErrorMessage = "A {0} deve ser pelo menos {2} e no maximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Campo obrigatorio!")]
        public bool RememberMe { get; set; }
    }
}
