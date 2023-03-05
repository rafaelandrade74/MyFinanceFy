namespace MyFinanceFy.Models
{
    public class PainelDadosRelSumModel
    {
        public string? IdPainel { get; set; }
        public string Categoria { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public int Ano { get; set; }        
        public decimal? JanValor { get; set; }
        public decimal? FevValor { get; set; }
        public decimal? MarValor { get; set; }
        public decimal? AbrValor { get; set; }
        public decimal? MaiValor { get; set; }
        public decimal? JunValor { get; set; }
        public decimal? JulValor { get; set; }
        public decimal? AgoValor { get; set; }
        public decimal? SetValor { get; set; }
        public decimal? OutValor { get; set; }
        public decimal? NovValor { get; set; }
        public decimal? DezValor { get; set; }
        public decimal? Total { get; set; }
    }
}
