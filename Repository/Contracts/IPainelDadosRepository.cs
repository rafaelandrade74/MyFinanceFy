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
        Task<QueryResult> DeleteByIdPainelAsync(string idPainel);
        /// <summary>
        /// Verifica se o usuario tem acesso ao painel de dados
        /// </summary>
        /// <param name="idPainelDados"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        Task<bool> UsuarioTemAcesso(string idPainelDados, string idUsuario);
    }
}