using Microsoft.EntityFrameworkCore;
using MyFinanceFy.Data;
using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Libs.Enums;
using MyFinanceFy.Models;
using MyFinanceFy.Repository.Contracts;

namespace MyFinanceFy.Repository
{
    public class PainelUsuarioRepository : RepositoryBase<PainelUsuario>, IPainelUsuarioRepository
    {
        private readonly ILogger<PainelRepository> _logger;
        public PainelUsuarioRepository(ApplicationDbContext _dbContext, ILogger<PainelRepository> logger) : base(_dbContext)
        {
            _logger = logger;
        }

        public async Task<QueryResult> CreateAsync(PainelUsuario painelUsuario)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Create(painelUsuario);
                    return new QueryResult(QueryResultStatus.Sucesso, "Usuario cadastrado ao painel com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao adicionar o usuario ao painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }
        public async Task<QueryResult> UpdateAsync(PainelUsuario painelUsuario)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Update(painelUsuario);
                    return new QueryResult(QueryResultStatus.Sucesso, "Alterações feitas com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao alterar dados do painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }
        public async Task<QueryResult> DeleteAsync(PainelUsuario painelUsuario)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Delete(painelUsuario);
                    return new QueryResult(QueryResultStatus.Sucesso, "Usuario removido do painel com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao deletar o usuario do painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }

        public async Task<IEnumerable<PainelUsuario>> FindAllAsync()
        {
            return await FindAll()
            .ToListAsync();
        }

        public IQueryable<PainelUsuario> FindAllWithIncludesAsQueryble()
        {
            return FindAll()
            .Include(x => x.Painel)!
            .ThenInclude(x=> x!.PainelDados)
            .Include(x => x.Usuario);
        }

        public async Task<IEnumerable<PainelUsuario>> FindAllWithIncludesAsync()
        {
            return await FindAllWithIncludesAsQueryble().ToListAsync();
        }

        public async Task<PainelUsuario?> FindByIdAsync(string Id)
        {
            return await FindByCondition(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PainelUsuario?>> FindByPainelIdAsync(string Id)
        {
            return await FindAllWithIncludesAsQueryble().Where(x => x.Painel!.Id == Id).ToListAsync();
        }

        public async Task<IEnumerable<PainelUsuario?>> FindByUsuarioIdAsync(string Id)
        {
            return await FindAllWithIncludesAsQueryble().Where(x => x.Usuario!.Id == Id).ToListAsync();
        }

        
    }
}
