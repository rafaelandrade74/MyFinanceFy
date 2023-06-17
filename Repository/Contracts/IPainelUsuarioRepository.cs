using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Models;

namespace MyFinanceFy.Repository.Contracts
{
    public interface IPainelUsuarioRepository : IRepositoryBase<PainelUsuario>
    {
        IQueryable<PainelUsuario> FindAllWithIncludesAsQueryble();
        Task<IEnumerable<PainelUsuario>> FindAllAsync();
        Task<IEnumerable<PainelUsuario>> FindAllWithIncludesAsync();
        Task<PainelUsuario?> FindByIdAsync(string Id);
        Task<IEnumerable<PainelUsuario?>> FindByPainelIdAsync(string Id);
        Task<IEnumerable<PainelUsuario?>> FindByUsuarioIdAsync(string Id);
        Task<QueryResult> CreateAsync(PainelUsuario painelUsuario);
        Task<QueryResult> UpdateAsync(PainelUsuario painelUsuario);
        Task<QueryResult> DeleteAsync(PainelUsuario painelUsuario);
        Task<QueryResult> DeleteByIdPainelAsync(string idPainel);
    }
}
