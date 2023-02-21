using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Models;

namespace MyFinanceFy.Repository.Contracts
{
    public interface ICategoriaRepository : IRepositoryBase<Categoria>
    {
        IQueryable<Categoria> FindAllWithIncludesAsQueryble();
        Task<IEnumerable<Categoria>> FindAllAsync();
        Task<IEnumerable<Categoria>> FindAllWithIncludesAsync();
        Task<Categoria?> FindByIdAsync(string Id);
        Task<QueryResult> CreateAsync(Categoria categoria);
        Task<QueryResult> UpdateAsync(Categoria categoria);
        Task<QueryResult> DeleteAsync(Categoria categoria);
    }
}