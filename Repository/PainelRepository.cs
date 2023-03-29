using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFinanceFy.Data;
using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Libs.Enums;
using MyFinanceFy.Models;
using MyFinanceFy.Repository.Contracts;

namespace MyFinanceFy.Repository
{
    public class PainelRepository : RepositoryBase<Painel>, IPainelRepository
    {
        private readonly ILogger<PainelRepository> _logger;
        public PainelRepository(ApplicationDbContext _dbContext, ILogger<PainelRepository> logger) : base(_dbContext)
        {
            _logger = logger;
        }

        public async Task<QueryResult> CreateAsync(Painel painel)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Create(painel);
                    return new QueryResult(QueryResultStatus.Sucesso, "Painel cadastrado com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao criar um novo painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }
        public async Task<QueryResult> UpdateAsync(Painel painel)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Update(painel);
                    return new QueryResult(QueryResultStatus.Sucesso, "Painel atualizado com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao atualizar um painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }

        public async Task<QueryResult> DeleteAsync(Painel painel)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Delete(painel);
                    return new QueryResult(QueryResultStatus.Sucesso, "Painel deletado com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao deletar o painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }

        public async Task<IEnumerable<Painel>> FindAllAsync()
        {
            return await FindAll()
            .ToListAsync();
        }

        public IQueryable<Painel> FindAllWithIncludesAsQueryble()
        {
            return FindAll()
            .Include(x => x.PainelDados)!
            .ThenInclude(x => x!.Categoria);
        }

        public async Task<IEnumerable<Painel>> FindAllWithIncludesAsync()
        {
            return await FindAllWithIncludesAsQueryble().ToListAsync();
        }


        public async Task<Painel?> FindByIdAsync(string Id)
        {
            return await FindByCondition(x=> x.Id == Id).FirstOrDefaultAsync();
        }
    }
}