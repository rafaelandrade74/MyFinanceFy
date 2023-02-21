using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Models;

namespace MyFinanceFy.Repository.Contracts
{
    public interface IPainelDadosRepository : IRepositoryBase<PainelDados>
    {
        IQueryable<PainelDados> FindAllWithIncludesAsQueryble();
        Task<IEnumerable<PainelDados>> FindAllAsync();
        Task<IEnumerable<PainelDados>> FindAllWithIncludesAsync();
        Task<PainelDados?> FindByIdAsync(string Id);
        Task<QueryResult> CreateAsync(PainelDados painelDados);
        Task<QueryResult> UpdateAsync(PainelDados painelDados);
        Task<QueryResult> DeleteAsync(PainelDados painelDados);
    }
}