using MyFinanceFy.Libs.Ajuda;
using MyFinanceFy.Models;

namespace MyFinanceFy.Repository.Contracts
{
    public interface IPainelRepository : IRepositoryBase<Painel>
    {
        IQueryable<Painel> FindAllWithIncludesAsQueryble();
        Task<IEnumerable<Painel>> FindAllAsync();
        Task<IEnumerable<Painel>> FindAllWithIncludesAsync();
        Task<Painel?> FindByIdAsync(string Id);
        Task<QueryResult> CreateAsync(Painel painel);
        Task<QueryResult> UpdateAsync(Painel painel);
        Task<QueryResult> DeleteAsync(Painel painel);
        /// <summary>
        /// Verifica se o usuario tem acesso ao painel
        /// </summary>
        /// <param name="idPainel"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        Task<bool> UsuarioTemAcesso(string idPainel, string idUsuario);
    }
}