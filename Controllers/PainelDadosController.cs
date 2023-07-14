using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFinanceFy.Data;
using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Libs.Enums;
using MyFinanceFy.Libs.Ext;
using MyFinanceFy.Models;
using MyFinanceFy.Repository.Contracts;

namespace MyFinanceFy.Controllers
{
    public class PainelDadosController : Controller
    {
        private readonly ILogger<PainelDadosController> _logger;
        private readonly IPainelDadosRepository _painelDadosRepository;
        private readonly IPainelRepository _painelRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ApplicationDbContext _applicationDb;

        public PainelDadosController(ILogger<PainelDadosController> logger, IPainelDadosRepository painelDadosRepository, ICategoriaRepository categoriaRepository, ApplicationDbContext applicationDb, IPainelRepository painelRepository)
        {
            _logger = logger;
            _painelDadosRepository = painelDadosRepository;
            _categoriaRepository = categoriaRepository;
            _applicationDb = applicationDb;
            _painelRepository = painelRepository;
        }
        /// <summary>
        /// Carrega Viewbags e TempDatas para a pagina de formulario
        /// </summary>
        /// <param name="ano"></param>
        /// <param name="painelDados"></param>
        /// <returns></returns>
        private async Task CarregarItensForm(int ano, PainelDados? painelDados)
        {
            ViewBag.Ano = ano;
            ViewBag.Categorias = (await _categoriaRepository.FindAllAsync()).Select(x => new SelectListItem(x.Nome, x.Id)).OrderBy(x => x.Text);
            TempData["UrlRemover"] = Url.Action(nameof(Remover));
            if (painelDados != null)
            {
                TempData["UrlRemoverRedirect"] = Url.Action(nameof(Index), new { Id = painelDados!.IdPainel });
                TempData["UrlDuplicar"] = Url.Action(nameof(Duplicar), new { painelDados.Id, ano });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Index(string Id, string? ano = null)
        {
            int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
            ViewBag.Ano = anoDt;

            if (string.IsNullOrEmpty(Id)) return RedirectToAction("Index", "Painel");
            if (!(await _painelRepository.UsuarioTemAcesso(Id, User.Id()))) return RedirectToAction("Index", "Painel");

            IEnumerable<PainelDadosRelModel>? painels = _applicationDb.PainelDadosView?
                .Where(x => x.IdPainel == Id && x.Ano == anoDt)
                .OrderByDescending(x => x.JanTipoSaldo)
                .OrderByDescending(x => x.FevTipoSaldo)
                .OrderByDescending(x => x.MarTipoSaldo)
                .OrderByDescending(x => x.AbrTipoSaldo)
                .OrderByDescending(x => x.MaiTipoSaldo)
                .OrderByDescending(x => x.JunTipoSaldo)
                .OrderByDescending(x => x.JulTipoSaldo)
                .OrderByDescending(x => x.AgoTipoSaldo)
                .OrderByDescending(x => x.SetTipoSaldo)
                .OrderByDescending(x => x.OutTipoSaldo)
                .OrderByDescending(x => x.NovTipoSaldo)
                .OrderByDescending(x => x.DezTipoSaldo)
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
        public async Task<IActionResult> Cadastrar(string IdPainel, string? ano = null)
        {
            int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
            await CarregarItensForm(anoDt, null);
            return View(new PainelDados() { IdPainel = IdPainel });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar([FromForm] PainelDados painelDados, string? ano = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
                    if (painelDados.StatusPagamento == StatusPagamento.Pago) painelDados.DataPagamento = DateTime.Now;
                    PainelDados? painelDadosProx = await _painelDadosRepository
                                .FindAllWithIncludesAsQueryble()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x =>
                                    x.DataFatura == painelDados.DataFatura
                                && x.IdPainel == painelDados.IdPainel
                                && x.Descricao == painelDados.Descricao
                                && x.IdCategoria == painelDados.IdCategoria);
                    if (painelDadosProx != null)
                    {
                        TempData["MSG_E"] = "Já existe!";
                        return View(painelDados);
                    }
                    for (int i = 1; i <= painelDados.Parcelas;)
                    {
                        var retorno = await _painelDadosRepository.CreateAsync(painelDados);
                        if (retorno.Status == QueryResultStatus.Sucesso) TempData["MSG_S"] = retorno.Mensagem;
                        else TempData["MSG_E"] = retorno.Mensagem;
                        painelDados.Id = null;
                        painelDados.Parcelas--;
                        painelDados.StatusPagamento = StatusPagamento.Pendente;
                        painelDados.DataPagamento = null;
                        painelDados.DataFatura = painelDados.DataFatura.AddMonths(1);
                    }

                    return RedirectToAction(nameof(Index), routeValues: new { Id = painelDados.IdPainel, ano });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar!");
                TempData["MSG_E"] = "Ocorreu um erro ao cadastrar";
                return View(painelDados);
            }
            finally
            {
                int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
                await CarregarItensForm(anoDt, null);
            }

            return View(painelDados);
        }
        public async Task<IActionResult> Editar(string Id, string? ano = null)
        {
            int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
            try
            {
                if (!(await _painelDadosRepository.UsuarioTemAcesso(Id, User.Id()))) return RedirectToAction("Index", "Painel");

                PainelDados? painelDados = await _painelDadosRepository.FindAllWithIncludesAsQueryble()
                    .FirstOrDefaultAsync(x => x.Id == Id);

                await CarregarItensForm(anoDt, painelDados);

                return View(painelDados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro!");
                TempData["MSG_E"] = "Ocorreu um erro ao carregar!";
                return RedirectToAction(nameof(Editar), new { Id });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar([FromForm] PainelDados painelDados, string? ano = null)
        {
            try
            {
                if (!ModelState.IsValid) return RedirectToAction(nameof(Editar), new { painelDados.Id });

                if (!(await _painelDadosRepository.UsuarioTemAcesso(painelDados.Id!, User.Id()))) return RedirectToAction("Index", "Painel");
                int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
                await CarregarItensForm(anoDt, painelDados);
                PainelDados? painelDadosProx = await _painelDadosRepository
                            .FindAllWithIncludesAsQueryble()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x =>
                                x.DataFatura == painelDados.DataFatura
                            && x.IdPainel == painelDados.IdPainel
                            && x.Descricao == painelDados.Descricao
                            && x.IdCategoria == painelDados.IdCategoria
                            && x.Id != painelDados.Id);
                if (painelDadosProx != null)
                {
                    TempData["MSG_E"] = "Já existe!";
                    return View(painelDados);
                }
                QueryResult? retorno = await _painelDadosRepository.UpdateAsync(painelDados);
                if (retorno.Status == QueryResultStatus.Sucesso) TempData["MSG_S"] = retorno.Mensagem;
                else TempData["MSG_E"] = retorno.Mensagem;
                return View(painelDados);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar");
                TempData["MSG_E"] = "Ocorreu um erro ao atualizar";
                return RedirectToAction(nameof(Editar), new { painelDados.Id });
            }
        }

        public async Task<IActionResult> Duplicar(string Id, string? ano = null)
        {
            int anoDt = ano == null ? DateTime.Now.Year : int.Parse(ano);
            try
            {
                if (!await _painelDadosRepository.UsuarioTemAcesso(Id, User.Id())) return RedirectToAction("Index", "Painel");
                ViewBag.Ano = anoDt;
                ViewBag.Categorias = (await _categoriaRepository.FindAllAsync())
                    .Select(x => new SelectListItem(x.Nome, x.Id))
                    .OrderBy(x => x.Text);
                PainelDados? painelDados = await _painelDadosRepository
                    .FindAllWithIncludesAsQueryble()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (painelDados == null) return RedirectToAction("Index", "Painel");
                DateOnly dtProximoMes = painelDados.DataFatura.AddMonths(1);
                PainelDados? painelDadosProx = await _painelDadosRepository
                    .FindAllWithIncludesAsQueryble()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.DataFatura == dtProximoMes
                    && x.IdPainel == painelDados.IdPainel
                    && x.Descricao == painelDados.Descricao
                    && x.IdCategoria == painelDados.IdCategoria);

                //se ja existir redireciona para o proximo existente.
                if (painelDadosProx != null) return RedirectToAction(nameof(Editar), new { Id = painelDadosProx?.Id ?? Id });

                PainelDados painelDadosNovo = new()
                {
                    IdCategoria = painelDados.IdCategoria,
                    IdPainel = painelDados.IdPainel,
                    Descricao = painelDados.Descricao,
                    Observacao = painelDados.Observacao,
                    Valor = painelDados.Valor,
                    ValorPago = 0,
                    Parcelas = painelDados.Parcelas == 1 ? painelDados.Parcelas : --painelDados.Parcelas,
                    TipoSaldo = painelDados.TipoSaldo,
                    DataFatura = painelDados.DataFatura.AddMonths(1),
                    StatusPagamento = StatusPagamento.Pendente
                };

                QueryResult? retorno = await _painelDadosRepository.CreateAsync(painelDadosNovo);
                if (retorno.Status == QueryResultStatus.Sucesso) TempData["MSG_S"] = retorno.Mensagem;
                else TempData["MSG_E"] = retorno.Mensagem;

                await CarregarItensForm(anoDt, painelDados);

                return RedirectToAction(nameof(Editar), new { Id = painelDadosNovo?.Id ?? Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro!");
                TempData["MSG_E"] = "Ocorreu um erro ao duplicar";
                return RedirectToAction(nameof(Editar), new { Id });
            }
        }
        [HttpDelete, ValidateAntiForgeryToken]
        public async Task<IActionResult> Remover(string Id)
        {
            try
            {
                if (Id.IsNullOrEmpty()) return BadRequest("Solicitação invalida!");

                var painel = await _painelDadosRepository.FindByIdAsync(Id);
                if (painel == null) return BadRequest("Solicitação invalida, não foi possivel remover!");
                if (!(await _painelDadosRepository.UsuarioTemAcesso(Id!, User.Id()))) return BadRequest("Solicitação invalida, não foi possivel remover!");
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