using Microsoft.EntityFrameworkCore;
using MyFinanceFy.Data;
using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Libs.Enums;
using MyFinanceFy.Models;
using MyFinanceFy.Repository.Contracts;

namespace MyFinanceFy.Repository
{
    public class PainelDadosRepository : RepositoryBase<PainelDados>, IPainelDadosRepository
    {
        private readonly ILogger<PainelDadosRepository> _logger;
        public PainelDadosRepository(ApplicationDbContext _dbContext, ILogger<PainelDadosRepository> logger) : base(_dbContext)
        {
            _logger = logger;
        }

        public async Task<QueryResult> CreateAsync(PainelDados painelDados)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Create(painelDados);
                    return new QueryResult(QueryResultStatus.Sucesso, "Fatura cadastrada com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao criar um novo painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }
        public async Task<QueryResult> UpdateAsync(PainelDados painelDados)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Update(painelDados);
                    return new QueryResult(QueryResultStatus.Sucesso, "Fatura atualizada com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao atualizar um painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }

        public async Task<QueryResult> DeleteAsync(PainelDados painelDados)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Delete(painelDados);
                    return new QueryResult(QueryResultStatus.Sucesso, "Fatura removida com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao deletar o painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }
        public async Task<QueryResult> DeleteByIdPainelAsync(string idPainel)
        {
            var listaPaineisDados = await FindByCondition(x=> x.IdPainel == idPainel).ToListAsync();
            foreach(var painelDados in listaPaineisDados) await DeleteAsync(painelDados);
            return new QueryResult(QueryResultStatus.Sucesso, "Faturas removidas com sucesso!");
        }

        public async Task<IEnumerable<PainelDados>> FindAllAsync()
        {
            return await FindAll()
            .ToListAsync();
        }

        public IQueryable<PainelDados> FindAllWithIncludesAsQueryble()
        {
            return FindAll()
            .Include(x => x!.Categoria);
        }

        public async Task<IEnumerable<PainelDados>> FindAllWithIncludesAsync()
        {
            return await FindAllWithIncludesAsQueryble().ToListAsync();
        }

        public async Task<PainelDados?> FindByIdAsync(string Id)
        {
            return await FindByCondition(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<bool> UsuarioTemAcesso(string idPainelDados, string idUsuario)
        {
            PainelDados? painelDados = await FindByCondition(x => x.Id == idPainelDados).FirstOrDefaultAsync();
            if (painelDados != null)
            {
                return await dbContext.PainelUsuarios!.AnyAsync(x => x.IdPainel == painelDados.IdPainel && x.IdUsuario == idUsuario);
            }
            return false;
        }


    }
}