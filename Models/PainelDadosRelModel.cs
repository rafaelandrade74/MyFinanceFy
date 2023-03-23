using MyFinanceFy.Libs.Enums;

namespace MyFinanceFy.Models
{
    public class PainelDadosRelModel
    {
        public string Categoria { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public int Ano { get; set; }
        public string? IdPainel { get; set; }
        public string? JanId { get; set; }
        public decimal? JanValor { get; set; }
        public decimal? JanValorPago { get; set; }
        public StatusPagamento? JanStatusPagamento { get; set; }
        public TipoSaldo? JanTipoSaldo { get; set; }
        public string? FevId { get; set; }
        public decimal? FevValor { get; set; }
        public decimal? FevValorPago { get; set; }
        public StatusPagamento? FevStatusPagamento { get; set; }
        public TipoSaldo? FevTipoSaldo { get; set; }
        public string? MarId { get; set; }
        public decimal? MarValor { get; set; }
        public decimal? MarValorPago { get; set; }
        public StatusPagamento? MarStatusPagamento { get; set; }
        public TipoSaldo? MarTipoSaldo { get; set; }
        public string? AbrId { get; set; }
        public decimal? AbrValor { get;set; }
        public decimal? AbrValorPago { get;set; }
        public StatusPagamento? AbrStatusPagamento { get; set; }
        public TipoSaldo? AbrTipoSaldo { get; set; }
        public string? MaiId { get; set; }
        public decimal? MaiValor { get; set; }
        public decimal? MaiValorPago { get; set; }
        public StatusPagamento? MaiStatusPagamento { get; set; }
        public TipoSaldo? MaiTipoSaldo { get; set; }
        public string? JunId { get; set; }
        public decimal? JunValor { get; set; }
        public decimal? JunValorPago { get; set; }
        public StatusPagamento? JunStatusPagamento { get; set; }
        public TipoSaldo? JunTipoSaldo { get; set; }
        public string? JulId { get; set; }
        public decimal? JulValor { get; set; }
        public decimal? JulValorPago { get; set; }
        public StatusPagamento? JulStatusPagamento { get; set; }
        public TipoSaldo? JulTipoSaldo { get; set; }
        public string? AgoId { get; set; }
        public decimal? AgoValor { get; set; }
        public decimal? AgoValorPago { get; set; }
        public StatusPagamento? AgoStatusPagamento { get; set; }
        public TipoSaldo? AgoTipoSaldo { get; set; }
        public string? SetId { get; set; }
        public decimal? SetValor { get; set; }
        public decimal? SetValorPago { get; set; }
        public StatusPagamento? SetStatusPagamento { get; set; }
        public TipoSaldo? SetTipoSaldo { get; set; }
        public string? OutId { get; set; }
        public decimal? OutValor { get; set; }
        public decimal? OutValorPago { get; set; }
        public StatusPagamento? OutStatusPagamento { get; set; }
        public TipoSaldo? OutTipoSaldo { get; set; }
        public string? NovId { get; set; }
        public decimal? NovValor { get; set; }
        public decimal? NovValorPago { get; set; }
        public StatusPagamento? NovStatusPagamento { get; set; }
        public TipoSaldo? NovTipoSaldo { get; set; }
        public string? DezId { get; set; }
        public decimal? DezValor { get; set; }
        public decimal? DezValorPago { get; set; }
        public StatusPagamento? DezStatusPagamento { get; set; }
        public TipoSaldo? DezTipoSaldo { get; set; }
    }
}
