using MyFinanceFy.Libs.Enums;

namespace MyFinanceFy.Libs.Ajuda
{
    public record QueryResult(QueryResultStatus Status, string Mensagem, Exception? Exception = null);
}