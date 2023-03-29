using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Libs.Enums;
using MyFinanceFy.Models;
using MyFinanceFy.Repository.Contracts;

namespace MyFinanceFy.Controllers
{
    public class PainelController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPainelRepository _painelRepository;
        private readonly IPainelUsuarioRepository _painelUsuarioRepository;
        private readonly IPainelDadosRepository _painelDadosRepository;        
        private readonly UserManager<Usuario> _userManager;

        public PainelController(ILogger<HomeController> logger, IPainelRepository painelRepository, UserManager<Usuario> userManager, IPainelDadosRepository painelDadosRepository, IPainelUsuarioRepository painelUsuarioRepository)
        {
            _logger = logger;
            _painelRepository = painelRepository;
            _userManager = userManager;
            _painelDadosRepository = painelDadosRepository;
            _painelUsuarioRepository = painelUsuarioRepository;
        }

        public async Task<IActionResult> Index()
        {
            string? idUsuario = _userManager.GetUserId(User);
            IEnumerable<Painel?> paineis = (await _painelUsuarioRepository.FindByUsuarioIdAsync(idUsuario ?? "")).Select(x=> x!.Painel);            
            
            TempData["UrlRemover"] = Url.Action(nameof(Remover));
            return View(paineis);
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar([FromForm] Painel painel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var retorno = await _painelRepository.CreateAsync(painel);
                    if (retorno.Status == QueryResultStatus.Sucesso) {
                        Usuario? usuarioAtual = await _userManager.GetUserAsync(User);
                        if (usuarioAtual != null) 
                        {
                            await _painelUsuarioRepository.CreateAsync(new PainelUsuario { IdPainel = painel.Id, IdUsuario = usuarioAtual.Id });
                            TempData["MSG_S"] = "Cadastrado com sucesso!";
                        }
                    }
                    else TempData["MSG_E"] = "Ocorreu um erro ao cadastrar";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                TempData["MSG_E"] = "Ocorreu um erro ao cadastrar";
                return View(painel);
            }

            return View(painel);
        }
        [HttpDelete, ValidateAntiForgeryToken]
        public async Task<IActionResult> Remover(string Id)
        {
            try
            {
                if (Id.IsNullOrEmpty()) return BadRequest("Solicitação invalida!");
                
                IEnumerable<PainelUsuario?>? painelUsuarios = await _painelUsuarioRepository.FindByPainelIdAsync(Id);
                if (painelUsuarios == null) return BadRequest("Solicitação invalida, não foi possivel remover!");
                
                foreach (var painelUsuario in painelUsuarios)
                {
                    foreach (var painelDados in painelUsuario!.Painel!.PainelDados!) await _painelDadosRepository.DeleteAsync(painelDados);
                    await _painelUsuarioRepository.DeleteAsync(painelUsuario);
                    
                }
                QueryResult result = await _painelRepository.DeleteAsync(painelUsuarios!.FirstOrDefault()!.Painel!);
                if (result.Status == QueryResultStatus.Sucesso) return Ok(result.Mensagem);
                return BadRequest(result.Mensagem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro!");
                return BadRequest("Ocorreu um erro sinistro! <br> " + ex.Message);
            }
        }

    }
}