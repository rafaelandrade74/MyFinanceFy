using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Libs.Enums;
using MyFinanceFy.Libs.Ext;
using MyFinanceFy.Models;
using MyFinanceFy.Repository.Contracts;
using System.Security.Claims;

namespace MyFinanceFy.Controllers
{
    public class PainelController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPainelRepository _painelRepository;
        private readonly IPainelUsuarioRepository _painelUsuarioRepository;
        private readonly IPainelDadosRepository _painelDadosRepository;

        public PainelController(ILogger<HomeController> logger, IPainelRepository painelRepository, IPainelDadosRepository painelDadosRepository, IPainelUsuarioRepository painelUsuarioRepository)
        {
            _logger = logger;
            _painelRepository = painelRepository;
            _painelDadosRepository = painelDadosRepository;
            _painelUsuarioRepository = painelUsuarioRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<PainelVisualizacao?> paineis = (await _painelUsuarioRepository.FindByUsuarioIdAsync(User.Id() ?? "")).Select(x => new PainelVisualizacao { Painel = x!.Painel!, Dono = x.Dono });
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
                    if (retorno.Status == QueryResultStatus.Sucesso)
                    {
                        await _painelUsuarioRepository.CreateAsync(new PainelUsuario { IdPainel = painel.Id, IdUsuario = User.Id(), Dono = true });
                        TempData["MSG_S"] = "Cadastrado com sucesso!";
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
                var painel = await _painelRepository.FindByIdAsync(Id);
                if(painel == null) return BadRequest("Solicitação invalida!");
                if (Id.IsNullOrEmpty()) return BadRequest("Solicitação invalida!");
                if (!(await _painelRepository.UsuarioTemAcesso(Id, User.Id()))) return BadRequest("Não foi possivel remover!");

                 await _painelUsuarioRepository.DeleteByIdPainelAsync(Id);
                await _painelDadosRepository.DeleteByIdPainelAsync(Id);
                var result = await _painelRepository.DeleteAsync(painel);
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