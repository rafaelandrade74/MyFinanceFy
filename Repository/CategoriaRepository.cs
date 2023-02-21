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
    public class CategoriaRepository : RepositoryBase<Categoria>, ICategoriaRepository
    {
        private readonly ILogger<PainelDadosRepository> _logger;
        public CategoriaRepository(ApplicationDbContext _dbContext, ILogger<PainelDadosRepository> logger) : base(_dbContext)
        {
            _logger = logger;
        }

        public async Task<QueryResult> CreateAsync(Categoria categoria)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Create(categoria);
                    return new QueryResult(QueryResultStatus.Sucesso, "Produto cadastrado com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao criar um novo painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }
        public async Task<QueryResult> UpdateAsync(Categoria categoria)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Update(categoria);
                    return new QueryResult(QueryResultStatus.Sucesso, "Produto cadastrado com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao atualizar um painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }

        public async Task<QueryResult> DeleteAsync(Categoria categoria)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Delete(categoria);
                    return new QueryResult(QueryResultStatus.Sucesso, "Produto cadastrado com sucesso!");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao deletar o painel");
                return new QueryResult(QueryResultStatus.Erro, "Ocorreu algum erro, entre em contato com o Admin!");
            }
        }

        public async Task<IEnumerable<Categoria>> FindAllAsync()
        {
            return await FindAll()
            .ToListAsync();
        }

        public IQueryable<Categoria> FindAllWithIncludesAsQueryble()
        {
            return FindAll();
        }

        public async Task<IEnumerable<Categoria>> FindAllWithIncludesAsync()
        {
            return await FindAllWithIncludesAsQueryble().ToListAsync();
        }

        public async Task<Categoria?> FindByIdAsync(string Id)
        {
            return await FindByCondition(x => x.Id == Id).FirstOrDefaultAsync();
        }
    }
}