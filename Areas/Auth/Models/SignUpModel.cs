using System.ComponentModel.DataAnnotations;


namespace MyFinanceFy.Areas.Auth.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Campo obrigatorio!")]
        [Display(Name = "Nome Completo")]
        [StringLength(100, ErrorMessage = "O campo {0} deve ser pelo menos {2} e no maximo {1} caracteres.", MinimumLength = 6)]
        public string FullName { get; set; }
        
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required(ErrorMessage = "Campo obrigatorio!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required(ErrorMessage = "Campo obrigatorio!")]
        [StringLength(100, ErrorMessage = "O campo {0} deve ser pelo menos {2} e no maximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        
        [Required(ErrorMessage = "Campo obrigatorio!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirme sua senha")]
        [Compare("Password", ErrorMessage = "A senha de confirmação deve ser igual a senha.")]
        public string ConfirmPassword { get; set; }
        public bool agreeTerms { get; } = true;
        [Required(ErrorMessage = "Campo obrigatorio!")]
        [Compare(nameof(agreeTerms), ErrorMessage = "Precisa aceitar os termos para cadastrar!")]
        public bool AgreeTerms { get; set; }
        
    }
}
