using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MyFinanceFy.Data;
using MyFinanceFy.Libs.Servicos;
using MyFinanceFy.Models;
using MyFinanceFy.Areas.Auth.Models;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Libs.Enums;
using MyFinanceFy.Libs.Ext;

namespace MyFinanceFy.Areas.Auth.Controllers
{
    [Area("Auth"), Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly EmailSender _emailSender;
        private readonly ApplicationDbContext dbContext;


        public AccountController(SignInManager<Usuario> signInManager, ILogger<AccountController> logger, UserManager<Usuario> userManager, UrlEncoder urlEncoder, EmailSender emailSender, ApplicationDbContext dbContext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _urlEncoder = urlEncoder;
            _emailSender = emailSender;
            this.dbContext = dbContext;
        }
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> SignUp([FromQuery] string? returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            ViewBag.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View();
        }
        [HttpPost, AutoValidateAntiforgeryToken, AllowAnonymous]
        public async Task<IActionResult> SignUp([FromForm] SignUpModel Input, [FromQuery] string? returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            ViewBag.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                Usuario? user = new() 
                {
                    FullName = Input.FullName
                };
                
                await _userManager.SetUserNameAsync(user, Input.Email);
                await _userManager.SetEmailAsync(user, Input.Email);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);                   
                    
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {

                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        string callbackUrl = Url.ActionLink(
                        "ConfirmEmail",
                        "Account",
                        values: new { area = "Auth", userId, code, returnUrl },
                        protocol: Request.Scheme) ?? "";
                        _emailSender.To(Input.Email);
                        await _emailSender.SendEmailAsync(
                            "Confirme sua conta",
                            $@"Caro {Input.FullName},<br><br>Para confirmar seu cadastro <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique aqui</a>.");

                        ModelState.AddModelError(string.Empty, "Enviamos um email para confirmar seu cadastro!");
                        return View();
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }
        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost, AutoValidateAntiforgeryToken, AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginModel login, [FromQuery] string? returnUrl)
        {
            if (!ModelState.IsValid) ModelState.AddModelError(string.Empty, "Usuário ou senha incorretos!");
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return LocalRedirect(returnUrl ?? "~/");
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("LoginWith2fa", new { returnUrl, rememberMe = true });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                ModelState.AddModelError(string.Empty, "Usuário esta bloqueado!");
            }
            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Usuário esta pendente de confirmação!");
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha incorretos!");
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout(string? returnUrl)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return LocalRedirect($"~/");
        }
        [HttpGet, Authorize]
        public async Task<IActionResult> Profile()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            string? userName = await _userManager.GetUserNameAsync(user);
            string? phoneNumberRaw = await _userManager.GetPhoneNumberAsync(user);
            
            string phoneCode = phoneNumberRaw != null ? phoneNumberRaw.Split(" ")[0] : "";
            string phoneNumber = phoneNumberRaw != null ? phoneNumberRaw.Split(" ")[1] : "";
            UserModel? Usuario = new()
            {
                Username = userName,
                PhoneCountryCode = phoneCode != "" ? (PhoneCountryCode)int.Parse(phoneCode) : PhoneCountryCode.Brazil,
                PhoneNumber = phoneNumber,
                FullName = user.FullName
            };
            
            return View(Usuario);
        }
        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Profile(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                userModel.PhoneNumber = (int)userModel.PhoneCountryCode + " " + Util.RemoveNaoNumericos(userModel.PhoneNumber!);
                Usuario? user = await _userManager.GetUserAsync(User);
                
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (userModel.PhoneNumber != phoneNumber)
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, userModel.PhoneNumber);
                    if (!setPhoneResult.Succeeded)
                    {
                        TempData["MSG_E"] = "Erro: Não foi possivel atualizar o número de telefone.";
                        return View();
                    }
                }
                
                user.FullName = userModel.FullName != user.FullName ? userModel.FullName : user.FullName;
                                
                var setResult = await _userManager.UpdateAsync(user);
                if (!setResult.Succeeded)
                {
                    TempData["MSG_E"] = "Ocorreu algum erro ao atualizar tente mais tarde!";
                    return View();
                }
                
                await _signInManager.RefreshSignInAsync(user);
                TempData["MSG_S"] = "Perfil atualizado!";
                return View();
            }
            TempData["MSG_E"] = "Ocorreu um erro ao atualizar!";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                //return RedirectToPage("./SetPassword");
            }

            return View();
        }
        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordModel changePassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new Exception($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            TempData["MSG_S"] = "Sua senha foi alterada com sucesso.";

            return RedirectToActionPermanent(nameof(Profile));
        }
        [HttpGet, AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost, AutoValidateAntiforgeryToken, AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordModel forgotPassword)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    TempData["MSG_S"] = "Foi enviado um email para alterar sua senha.";
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                string? code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                string callbackUrl = Url.ActionLink(
                    nameof(ResetPassword),
                    "Account",
                    values: new { area = "Auth", code, email = forgotPassword.Email },
                    protocol: Request.Scheme) ?? "";
                _emailSender.To(forgotPassword.Email);
                await _emailSender.SendEmailAsync(
                    "Resetar Senha",
                    $@"Caro cliente,<br><br>Para trocar a senha <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique aqui</a>.");
                TempData["MSG_S"] = "Foi enviado um email para alterar sua senha.";
                return View();
            }

            return View();
        }
        [HttpGet, AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet, AllowAnonymous]
        public IActionResult ResetPassword(string code, string email)
        {
            var reset = new ResetPasswordModel() { Code = code, Email = email };
            return View(reset);
        }
        [HttpPost, AutoValidateAntiforgeryToken, AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPassword.Code));

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, code, resetPassword.Password);
                if (result.Succeeded)
                {
                    return RedirectToActionPermanent(nameof(Login));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado!");
            }
            return View();
        }
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(string returnUrl, bool rememberMe)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            TempData["returnUrl"] = returnUrl;
            TempData["rememberMe"] = rememberMe;
            return View();
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(AuthenticatorModel authenticatorModel, string ReturnUrl)
        {
            ReturnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
                if (user == null)
                {
                    throw new InvalidOperationException($"Unable to load two-factor authentication user.");
                }

                var authenticatorCode = authenticatorModel.TwoFactorCode!.Replace(" ", string.Empty).Replace("-", string.Empty);

                var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, true, authenticatorModel.RememberMachine);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                    return LocalRedirect(ReturnUrl);
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                    ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            TwoFactorAuthenticationModel twoFactorAuthenticationModel = new()
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            };
            return View(twoFactorAuthenticationModel);
        }
        [HttpPost]
        public async Task<IActionResult> ForgetThisBrowser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _signInManager.ForgetTwoFactorClientAsync();
            TempData["MSG_S"] = "O navegador atual foi esquecido. Quando você fizer login novamente a partir deste navegador, você será solicitado a fornecer seu código 2fa.";
            return RedirectToAction(nameof(TwoFactorAuthentication));
        }
        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var enable = await LoadSharedKeyAndQrCodeUriAsync(user);
            return View(enable);
        }

        [HttpPost]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorModel enableAuthenticatorModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return View(enableAuthenticatorModel);
            }

            // Strip spaces and hyphens
            var verificationCode = enableAuthenticatorModel.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                TempData["MSG_E"] = "Error - Verification code is invalid.";
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return View(enableAuthenticatorModel);
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            var userId = await _userManager.GetUserIdAsync(user);
            _logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", userId);

            TempData["MSG_S"] = "Autenticador habilitado com sucesso!";

            if (await _userManager.CountRecoveryCodesAsync(user) == 0)
            {
                var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                TempData["RecoveryCodes"] = recoveryCodes.ToArray();
                return RedirectToAction("ShowRecoveryCodes");
            }
            else
            {
                return RedirectToAction("TwoFactorAuthentication");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Disable2fa()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred disabling 2FA.");
            }

            _logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", _userManager.GetUserId(User));
            TempData["MSG_S"] = "Segunda autenticação foi desabilitada!";
            return RedirectToAction("TwoFactorAuthentication");
        }
        [HttpPost]
        public async Task<IActionResult> ResetAuthenticator()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            string? userId = await _userManager.GetUserIdAsync(user);
            _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

            await _signInManager.RefreshSignInAsync(user);
            TempData["MSG_S"] = "Autenticador resetado com sucesso!";

            return RedirectToAction("EnableAuthenticator");
        }
        [HttpPost]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!isTwoFactorEnabled)
            {
                throw new InvalidOperationException($"Cannot generate recovery codes for user as they do not have 2FA enabled.");
            }

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            TempData["RecoveryCodes"] = recoveryCodes.ToArray();

            _logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", userId);
            TempData["MSG_S"] = "Novos codigos gerados com sucesso!";
            return RedirectToAction("ShowRecoveryCodes");
        }
        [HttpGet]
        public async Task<IActionResult> ShowRecoveryCodes()
        {
            string[]? recoveryCodes = TempData["RecoveryCodes"] as string[];
            var internalLoginProvider = "[AspNetUserStore]";
            var recoveryCodeTokenName = "RecoveryCodes";

            if (recoveryCodes == null || recoveryCodes.Length == 0)
            {
                var user = await _userManager.GetUserAsync(User);
                var token = await _userManager.GetAuthenticationTokenAsync(user, internalLoginProvider, recoveryCodeTokenName);
                recoveryCodes = token.Split(";", StringSplitOptions.RemoveEmptyEntries);
            }

            return View(recoveryCodes);
        }
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string? returnUrl)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            TempData["returnUrl"] = returnUrl;

            return View();
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode([FromForm] IFormCollection form)
        {
            string? recoveryCode = form.Where(x => x.Key == "RecoveryCode")
                .Select(x => x.Value.ToString() ?? "")
                .First()
                .Replace(" ", string.Empty);
            var returnUrl = form.Where(x => x.Key == "ReturnUrl")
                .Select(x => x.Value.ToString() ?? "")
                .First()
                .Replace(" ", string.Empty);
            if (string.IsNullOrEmpty(recoveryCode))
            {
                return View();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            var userId = await _userManager.GetUserIdAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToAction("Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        #region Ajudas
        private async Task<EnableAuthenticatorModel> LoadSharedKeyAndQrCodeUriAsync(Usuario user)
        {
            // Load the authenticator key & QR code URI to display on the form
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }
            string? email = await _userManager.GetEmailAsync(user);
            return new()
            {
                SharedKey = FormatKey(unformattedKey),
                AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey)
            };
        }
        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
            CultureInfo.InvariantCulture,
                "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6",
                _urlEncoder.Encode(Constantes.MyFinancefy),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
        #endregion
    }
}
