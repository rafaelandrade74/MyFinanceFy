using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinanceFy.Migrations
{
    /// <inheritdoc />
    public partial class CriandoViewsPainelDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW view_fin_painel_dados AS
SELECT
fpd.Id,
fc.Nome AS Categoria,
fpd.Descricao,
CASE 
	WHEN fpd.TipoSaldo = 1 THEN fpd.Valor
	ELSE fpd.Valor * -1
END Valor,
CASE 
	WHEN fpd.TipoSaldo = 1 THEN fpd.ValorPago
	ELSE fpd.ValorPago * -1
END ValorPago,
fpd.TipoSaldo,
fpd.StatusPagamento,
EXTRACT(YEAR FROM fpd.DataFatura) AS Ano,
Month(fpd.DataFatura) AS Mes,
fpd.IdPainel 
FROM fin_painel_dados fpd
LEFT JOIN fin_categorias fc ON fc.Id = fpd.IdCategoria");
            migrationBuilder.Sql(@"CREATE VIEW view_fin_painel_dados_cat AS
SELECT DISTINCT b.Categoria, b.Descricao, b.Ano,b.TipoSaldo, b.IdPainel FROM view_fin_painel_dados b");
            migrationBuilder.Sql(@"CREATE VIEW view_fin_painel_dados_rel AS
SELECT 
a.*,
CASE 
	WHEN jan.Id IS NOT NULL THEN jan.Id 
	ELSE """"""
END JanId,
CASE 
	WHEN jan.Valor IS NOT NULL THEN jan.Valor 
	ELSE 0
END JanValor,
CASE 
	WHEN jan.ValorPago IS NOT NULL THEN jan.ValorPago 
	ELSE 0
END JanValorPago,
CASE WHEN jan.StatusPagamento IS NOT NULL THEN jan.StatusPagamento ELSE 0 END JanStatusPagamento,
CASE WHEN jan.TipoSaldo IS NOT NULL THEN jan.TipoSaldo ELSE 0 END JanTipoSaldo,
CASE 
	WHEN fev.Id IS NOT NULL THEN fev.Id 
	ELSE """"
END FevId,
CASE 
	WHEN fev.Valor IS NOT NULL THEN fev.Valor 
	ELSE 0
END FevValor,
CASE 
	WHEN fev.ValorPago IS NOT NULL THEN fev.ValorPago 
	ELSE 0
END FevValorPago,
CASE WHEN fev.StatusPagamento IS NOT NULL THEN fev.StatusPagamento ELSE 0 END FevStatusPagamento,
CASE WHEN fev.TipoSaldo IS NOT NULL THEN fev.TipoSaldo ELSE 0 END FevTipoSaldo,
CASE 
	WHEN mar.Id IS NOT NULL THEN mar.Id 
	ELSE """"
END MarId,
CASE 
	WHEN mar.Valor IS NOT NULL THEN mar.Valor 
	ELSE 0
END MarValor,
CASE 
	WHEN mar.ValorPago IS NOT NULL THEN mar.ValorPago 
	ELSE 0
END MarValorPago,
CASE WHEN mar.StatusPagamento IS NOT NULL THEN mar.StatusPagamento	ELSE 0 END MarStatusPagamento,
CASE WHEN mar.TipoSaldo IS NOT NULL THEN mar.TipoSaldo ELSE 0 END MarTipoSaldo,
CASE 
	WHEN abr.Id IS NOT NULL THEN abr.Id 
	ELSE """"
END AbrId,
CASE 
	WHEN abr.Valor IS NOT NULL THEN abr.Valor 
	ELSE 0
END AbrValor,
CASE 
	WHEN abr.ValorPago IS NOT NULL THEN abr.ValorPago 
	ELSE 0
END AbrValorPago,
CASE 
	WHEN abr.StatusPagamento IS NOT NULL THEN abr.StatusPagamento
	ELSE 0
END AbrStatusPagamento,
CASE WHEN abr.TipoSaldo IS NOT NULL THEN abr.TipoSaldo ELSE 0 END AbrTipoSaldo,
CASE 
	WHEN mai.Id IS NOT NULL THEN mai.Id 
	ELSE """"
END MaiId,
CASE 
	WHEN mai.Valor IS NOT NULL THEN mai.Valor 
	ELSE 0
END MaiValor,
CASE 
	WHEN mai.ValorPago IS NOT NULL THEN mai.ValorPago 
	ELSE 0
END MaiValorPago,
CASE 
	WHEN mai.StatusPagamento IS NOT NULL THEN mai.StatusPagamento
	ELSE 0
END MaiStatusPagamento,
CASE WHEN mai.TipoSaldo IS NOT NULL THEN mai.TipoSaldo ELSE 0 END MaiTipoSaldo,
CASE 
	WHEN jun.Id IS NOT NULL THEN jun.Id 
	ELSE """"
END JunId,
CASE 
	WHEN jun.Valor IS NOT NULL THEN jun.Valor 
	ELSE 0
END JunValor,
CASE 
	WHEN jun.ValorPago IS NOT NULL THEN jun.ValorPago 
	ELSE 0
END JunValorPago,
CASE 
	WHEN jun.StatusPagamento IS NOT NULL THEN jun.StatusPagamento
	ELSE 0
END JunStatusPagamento,
CASE WHEN jun.TipoSaldo IS NOT NULL THEN jun.TipoSaldo ELSE 0 END JunTipoSaldo,
CASE 
	WHEN jul.Id IS NOT NULL THEN jul.Id 
	ELSE """"
END JulId,
CASE 
	WHEN jul.Valor IS NOT NULL THEN jul.Valor 
	ELSE 0
END JulValor,
CASE 
	WHEN jul.ValorPago IS NOT NULL THEN jul.ValorPago 
	ELSE 0
END JulValorPago,
CASE 
	WHEN jul.StatusPagamento IS NOT NULL THEN jul.StatusPagamento
	ELSE 0
END JulStatusPagamento,
CASE WHEN jul.TipoSaldo IS NOT NULL THEN jul.TipoSaldo ELSE 0 END JulTipoSaldo,
CASE 
	WHEN ago.Id IS NOT NULL THEN ago.Id 
	ELSE """"
END AgoId,
CASE 
	WHEN ago.Valor IS NOT NULL THEN ago.Valor 
	ELSE 0
END AgoValor,
CASE 
	WHEN ago.ValorPago IS NOT NULL THEN ago.ValorPago 
	ELSE 0
END AgoValorPago,
CASE 
	WHEN ago.StatusPagamento IS NOT NULL THEN ago.StatusPagamento 
	ELSE 0 
END AgoStatusPagamento,
CASE WHEN ago.TipoSaldo IS NOT NULL THEN ago.TipoSaldo ELSE 0 END AgoTipoSaldo,
CASE 
	WHEN setem.Id IS NOT NULL THEN setem.Id 
	ELSE """"
END SetId,
CASE 
	WHEN setem.Valor IS NOT NULL THEN setem.Valor 
	ELSE 0
END SetValor,
CASE 
	WHEN setem.ValorPago IS NOT NULL THEN setem.ValorPago 
	ELSE 0
END SetValorPago,
CASE 
	WHEN setem.StatusPagamento IS NOT NULL THEN setem.StatusPagamento 
	ELSE 0 
END SetStatusPagamento,
CASE WHEN setem.TipoSaldo IS NOT NULL THEN setem.TipoSaldo ELSE 0 END SetTipoSaldo,
CASE 
	WHEN outu.Id IS NOT NULL THEN outu.Id 
	ELSE """"
END OutId,
CASE 
	WHEN outu.Valor IS NOT NULL THEN outu.Valor 
	ELSE 0
END OutValor,
CASE 
	WHEN outu.ValorPago IS NOT NULL THEN outu.ValorPago 
	ELSE 0
END OutValorPago,
CASE 
	WHEN outu.StatusPagamento IS NOT NULL THEN outu.StatusPagamento
	ELSE 0
END OutStatusPagamento,
CASE WHEN outu.TipoSaldo IS NOT NULL THEN outu.TipoSaldo ELSE 0 END OutTipoSaldo,
CASE 
	WHEN nov.Id IS NOT NULL THEN nov.Id 
	ELSE """"
END NovId,
CASE 
	WHEN nov.Valor IS NOT NULL THEN nov.Valor 
	ELSE 0
END NovValor,
CASE 
	WHEN nov.ValorPago IS NOT NULL THEN nov.ValorPago 
	ELSE 0
END NovValorPago,
CASE 
	WHEN nov.StatusPagamento IS NOT NULL THEN nov.StatusPagamento
	ELSE 0
END NovStatusPagamento,
CASE WHEN nov.TipoSaldo IS NOT NULL THEN nov.TipoSaldo ELSE 0 END NovTipoSaldo,
CASE 
	WHEN dez.Id IS NOT NULL THEN dez.Id 
	ELSE """"
END DezId,
CASE 
	WHEN dez.Valor IS NOT NULL THEN dez.Valor 
	ELSE 0
END DezValor,
CASE 
	WHEN dez.ValorPago IS NOT NULL THEN dez.ValorPago 
	ELSE 0
END DezValorPago,
CASE 
	WHEN dez.StatusPagamento IS NOT NULL THEN dez.StatusPagamento
	ELSE 0
END DezStatusPagamento,
CASE WHEN dez.TipoSaldo IS NOT NULL THEN dez.TipoSaldo ELSE 0 END DezTipoSaldo
FROM view_fin_painel_dados_cat a
LEFT JOIN view_fin_painel_dados jan 
	ON jan.IdPainel = a.IdPainel 
		AND jan.Ano = a.Ano 
		AND jan.Categoria = a.Categoria 
		AND jan.Descricao = a.Descricao
		AND jan.TipoSaldo = a.TipoSaldo
		AND jan.Mes = 1
LEFT JOIN view_fin_painel_dados fev 
	ON fev.IdPainel = a.IdPainel 
		AND fev.Ano = a.Ano 
		AND fev.Categoria = a.Categoria 
		AND fev.Descricao = a.Descricao 
		AND fev.TipoSaldo = a.TipoSaldo
		AND fev.Mes = 2
LEFT JOIN view_fin_painel_dados mar 
	ON mar.IdPainel = a.IdPainel 
		AND mar.Ano = a.Ano 
		AND mar.Categoria = a.Categoria 
		AND mar.Descricao = a.Descricao 
		AND mar.TipoSaldo = a.TipoSaldo
		AND mar.Mes = 3
LEFT JOIN view_fin_painel_dados abr 
	ON abr.IdPainel = a.IdPainel 
		AND abr.Ano = a.Ano 
		AND abr.Categoria = a.Categoria 
		AND abr.Descricao = a.Descricao 
		AND abr.TipoSaldo = a.TipoSaldo
		AND abr.Mes = 4
LEFT JOIN view_fin_painel_dados mai 
	ON mai.IdPainel = a.IdPainel 
		AND mai.Ano = a.Ano 
		AND mai.Categoria = a.Categoria 
		AND mai.Descricao = a.Descricao
		AND mai.TipoSaldo = a.TipoSaldo
		AND mai.Mes = 5
LEFT JOIN view_fin_painel_dados jun 
	ON jun.IdPainel = a.IdPainel 
		AND jun.Ano = a.Ano 
		AND jun.Categoria = a.Categoria 
		AND jun.Descricao = a.Descricao 
		AND jun.TipoSaldo = a.TipoSaldo
		AND jun.Mes = 6
LEFT JOIN view_fin_painel_dados jul 
	ON jul.IdPainel = a.IdPainel 
		AND jul.Ano = a.Ano 
		AND jul.Categoria = a.Categoria 
		AND jul.Descricao = a.Descricao 
		AND jul.TipoSaldo = a.TipoSaldo
		AND jul.Mes = 7
LEFT JOIN view_fin_painel_dados ago 
	ON ago.IdPainel = a.IdPainel 
		AND ago.Ano = a.Ano 
		AND ago.Categoria = a.Categoria 
		AND ago.Descricao = a.Descricao 
		AND ago.TipoSaldo = a.TipoSaldo
		AND ago.Mes = 8
LEFT JOIN view_fin_painel_dados setem 
	ON setem.IdPainel = a.IdPainel 
		AND setem.Ano = a.Ano 
		AND setem.Categoria = a.Categoria 
		AND setem.Descricao = a.Descricao 
		AND setem.TipoSaldo = a.TipoSaldo
		AND setem.Mes = 9
LEFT JOIN view_fin_painel_dados outu 
	ON outu.IdPainel = a.IdPainel 
		AND outu.Ano = a.Ano 
		AND outu.Categoria = a.Categoria 
		AND outu.Descricao = a.Descricao 
		AND outu.TipoSaldo = a.TipoSaldo
		AND outu.Mes = 10
LEFT JOIN view_fin_painel_dados nov 
	ON nov.IdPainel = a.IdPainel 
		AND nov.Ano = a.Ano 
		AND nov.Categoria = a.Categoria 
		AND nov.Descricao = a.Descricao 
		AND nov.TipoSaldo = a.TipoSaldo
		AND nov.Mes = 11
LEFT JOIN view_fin_painel_dados dez 
	ON dez.IdPainel = a.IdPainel 
		AND dez.Ano = a.Ano 
		AND dez.Categoria = a.Categoria 
		AND dez.Descricao = a.Descricao 
		AND dez.TipoSaldo = a.TipoSaldo
		AND dez.Mes = 12");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW view_fin_painel_dados");
            migrationBuilder.Sql("DROP VIEW view_fin_painel_dados_cat");
            migrationBuilder.Sql("DROP VIEW view_fin_painel_dados_rel");
        }
    }
}
