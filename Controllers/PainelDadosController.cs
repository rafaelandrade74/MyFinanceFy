﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFinanceFy.Data;
using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Libs.Enums;
using MyFinanceFy.Models;
using MyFinanceFy.Repository.Contracts;
using System.Globalization;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyFinanceFy.Controllers
{
    public class PainelDadosController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPainelDadosRepository _painelDadosRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ApplicationDbContext _applicationDb;
        private readonly UserManager<Usuario> _userManager;

        public PainelDadosController(ILogger<HomeController> logger, IPainelDadosRepository painelDadosRepository, UserManager<Usuario> userManager, ICategoriaRepository categoriaRepository, ApplicationDbContext applicationDb)
        {
            _logger = logger;
            _painelDadosRepository = painelDadosRepository;
            _userManager = userManager;
            _categoriaRepository = categoriaRepository;
            _applicationDb = applicationDb;
        }
        [HttpGet]
        public IActionResult Index(string Id, string? ano = null)
        {
            int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
            ViewBag.Ano = anoDt;

            if (string.IsNullOrEmpty(Id)) return RedirectToAction("Index", "Painel");

            IEnumerable<PainelDadosRelModel>? painels = _applicationDb.PainelDadosView?
                .Where(x => x.IdPainel == Id && x.Ano == anoDt)
                .OrderByDescending(x=> x.JanTipoSaldo)
                .OrderByDescending(x=> x.FevTipoSaldo)
                .OrderByDescending(x=> x.MarTipoSaldo)
                .OrderByDescending(x=> x.AbrTipoSaldo)
                .OrderByDescending(x=> x.MaiTipoSaldo)
                .OrderByDescending(x=> x.JunTipoSaldo)
                .OrderByDescending(x=> x.JulTipoSaldo)
                .OrderByDescending(x=> x.AgoTipoSaldo)
                .OrderByDescending(x=> x.SetTipoSaldo)
                .OrderByDescending(x=> x.OutTipoSaldo)
                .OrderByDescending(x=> x.NovTipoSaldo)
                .OrderByDescending(x=> x.DezTipoSaldo)
                .OrderByDescending(x => x.Categoria)
                .ToList();

            ViewBag.IdPainel = Id;
            List<int>? listaAnosBase = _applicationDb.PainelDadosView?.Where(x => x.IdPainel == Id).Select(x => x.Ano).Distinct().ToList();
            listaAnosBase ??= new List<int>();
            if (!listaAnosBase!.Any()) listaAnosBase.Add(DateTime.Now.Year);
            var anoMax = listaAnosBase!.Max() + 1;
            for (int i = anoMax; i < anoMax + 3; i++)
            {
                listaAnosBase.Add(i);
            }
            ViewBag.ListaAnos = listaAnosBase!.Select(x => new SelectListItem(x.ToString(), x.ToString())).OrderBy(x => x.Text).ToList();
            return View(new PainelDadosRelFinalModel { PainelDadosRel = painels! });
        }
        [HttpGet]
        public async Task<IActionResult> Cadastrar(string IdPainel,string? ano = null)
        {
            int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
            ViewBag.Ano = anoDt;
            ViewBag.Categorias = (await _categoriaRepository.FindAllAsync()).Select(x => new SelectListItem(x.Nome, x.Id)).OrderBy(x => x.Text);
            return View(new PainelDados() { IdPainel = IdPainel });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar([FromForm] PainelDados painel, string? ano = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
                    if (painel.StatusPagamento == StatusPagamento.Pago) painel.DataPagamento = DateTime.Now;
                    for (int i = 1; i <= painel.Parcelas;)
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

                    return RedirectToAction(nameof(Index), routeValues: new { Id = painel.IdPainel , ano});
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
        public async Task<IActionResult> Editar(string Id, string? ano = null)
        {
            int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
            ViewBag.Ano = anoDt;
            ViewBag.Categorias = (await _categoriaRepository.FindAllAsync()).Select(x => new SelectListItem(x.Nome, x.Id)).OrderBy(x => x.Text);
            var painelDados = await _painelDadosRepository.FindAllWithIncludesAsQueryble().FirstOrDefaultAsync(x => x.Id == Id);
            TempData["UrlRemover"] = Url.Action(nameof(Remover));
            TempData["UrlRemoverRedirect"] = Url.Action(nameof(Index), new { Id = painelDados!.IdPainel });

            return View(painelDados);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar([FromForm] PainelDados painel, string? ano = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var retorno = await _painelDadosRepository.UpdateAsync(painel);
                    if (retorno.Status == QueryResultStatus.Sucesso) TempData["MSG_S"] = retorno.Mensagem;
                    else TempData["MSG_E"] = retorno.Mensagem;
                    return RedirectToAction(nameof(Index), routeValues: new { Id = painel.IdPainel, ano });
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