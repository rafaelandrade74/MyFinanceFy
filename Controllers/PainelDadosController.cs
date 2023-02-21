using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Libs.Enums;
using MyFinanceFy.Models;
using MyFinanceFy.Repository.Contracts;
using System.Globalization;

namespace MyFinanceFy.Controllers
{
    public class PainelDadosController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPainelDadosRepository _painelDadosRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly UserManager<Usuario> _userManager;

        public PainelDadosController(ILogger<HomeController> logger, IPainelDadosRepository painelDadosRepository, UserManager<Usuario> userManager, ICategoriaRepository categoriaRepository)
        {
            _logger = logger;
            _painelDadosRepository = painelDadosRepository;
            _userManager = userManager;
            _categoriaRepository = categoriaRepository;
        }
        [HttpGet]
        public IActionResult Index(string Id, string? mes = null, string? ano = null)
        {
            int mesDt = mes == null ? DateTime.Now.Month : int.Parse(mes);
            int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
            ViewBag.Mes = mesDt;
            ViewBag.Ano = anoDt;
            
            if (string.IsNullOrEmpty(Id)) return RedirectToAction("Index", "Painel");
            IEnumerable<PainelDados?> painels = _painelDadosRepository.FindAllWithIncludesAsQueryble()
                .Where(x => x.IdPainel == Id && x.DataFatura.Month == mesDt && x.DataFatura.Year == anoDt)
                .ToList();
            TempData["UrlRemover"] = Url.Action(nameof(Remover));
            ViewBag.IdPainel = Id;
            return View(painels);
        }
        [HttpGet]
        public async Task<IActionResult> Cadastrar(string IdPainel)
        {
            ViewBag.Categorias = (await _categoriaRepository.FindAllAsync()).Select(x => new SelectListItem(x.Nome, x.Id)).OrderBy(x => x.Text);
            return View(new PainelDados() { IdPainel = IdPainel });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar([FromForm] PainelDados painel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (painel.StatusPagamento == StatusPagamento.Pago) painel.DataPagamento = DateTime.Now;
                    for (int i = 1; i <= painel.Parcelas; )
                    {
                        var retorno = await _painelDadosRepository.CreateAsync(painel);
                        if (retorno.Status == QueryResultStatus.Sucesso) TempData["MSG_S"] = retorno.Mensagem;
                        else TempData["MSG_E"] = retorno.Mensagem;
                        painel.Id = null;
                        painel.Parcelas--;
                        painel.StatusPagamento = StatusPagamento.Pendente;
                        painel.DataPagamento = null;
                        painel.DataFatura = painel.DataFatura.AddMonths(1);
                    }
                    
                    return RedirectToAction(nameof(Index), routeValues: new { Id = painel.IdPainel });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                TempData["MSG_E"] = "Ocorreu um erro ao cadastrar";
                return View(painel);
            }
            finally
            {
                ViewBag.Categorias = (await _categoriaRepository.FindAllAsync()).Select(x => new SelectListItem(x.Nome, x.Id)).OrderBy(x => x.Text);
            }

            return View(painel);
        }
        public async Task<IActionResult> Editar(string Id)
        {
            ViewBag.Categorias = (await _categoriaRepository.FindAllAsync()).Select(x => new SelectListItem(x.Nome, x.Id)).OrderBy(x => x.Text);
            var painelDados = await _painelDadosRepository.FindAllWithIncludesAsQueryble().FirstOrDefaultAsync(x=> x.Id == Id);
            return View(painelDados);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar([FromForm] PainelDados painel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var retorno = await _painelDadosRepository.UpdateAsync(painel);
                    if (retorno.Status == QueryResultStatus.Sucesso) TempData["MSG_S"] = retorno.Mensagem;
                    else TempData["MSG_E"] = retorno.Mensagem;
                    return RedirectToAction(nameof(Index), routeValues: new { Id = painel.IdPainel });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                TempData["MSG_E"] = "Ocorreu um erro ao atualizar";
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

                var painel = await _painelDadosRepository.FindByIdAsync(Id);
                if (painel == null) return BadRequest("Solicitação invalida, não foi possivel remover!");

                QueryResult result = await _painelDadosRepository.DeleteAsync(painel);
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