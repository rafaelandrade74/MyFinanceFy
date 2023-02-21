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
        private readonly IPainelDadosRepository _painelDadosRepository;
        private readonly UserManager<Usuario> _userManager;

        public PainelController(ILogger<HomeController> logger, IPainelRepository painelRepository, UserManager<Usuario> userManager, IPainelDadosRepository painelDadosRepository)
        {
            _logger = logger;
            _painelRepository = painelRepository;
            _userManager = userManager;
            _painelDadosRepository = painelDadosRepository;
        }

        public async Task<IActionResult> Index()
        {
            string? idUsuario = _userManager.GetUserId(User);
            IEnumerable<Painel>? paineis = await _painelRepository.FindByUserIdAsync(idUsuario ?? "");
            TempData["UrlRemover"] = Url.Action(nameof(Remover));
            return View(paineis);
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View(new Painel() { IdUsuario = _userManager.GetUserId(User)! });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar([FromForm] Painel painel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var retorno = await _painelRepository.CreateAsync(painel);
                    if (retorno.Status == QueryResultStatus.Sucesso) TempData["MSG_S"] = "Cadastrado com sucesso!";
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

                var painel = await _painelRepository.FindByIdAsync(Id);
                if (painel == null) return BadRequest("Solicitação invalida, não foi possivel remover!");
                var painelsDados = await _painelDadosRepository
                    .FindByCondition(x => x.IdPainel == painel.Id)
                    .ToListAsync();

                foreach (var painelDados in painelsDados) await _painelDadosRepository.DeleteAsync(painelDados);

                QueryResult result = await _painelRepository.DeleteAsync(painel);
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