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
        public string? FevId { get; set; }
        public decimal? FevValor { get; set; }
        public string? MarId { get; set; }
        public decimal? MarValor { get; set; }
        public string? AbrId { get; set; }
        public decimal? AbrValor { get;set; }
        public string? MaiId { get; set; }
        public decimal? MaiValor { get; set; }
        public string? JunId { get; set; }
        public decimal? JunValor { get; set; }
        public string? JulId { get; set; }
        public decimal? JulValor { get; set; }
        public string? AgoId { get; set; }
        public decimal? AgoValor { get; set; }
        public string? SetId { get; set; }
        public decimal? SetValor { get; set; }
        public string? OutId { get; set; }
        public decimal? OutValor { get; set; }
        public string? NovId { get; set; }
        public decimal? NovValor { get; set; }
        public string? DezId { get; set; }
        public decimal? DezValor { get; set; }
    }
}
