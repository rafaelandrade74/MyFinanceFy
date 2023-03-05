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
CASE fpd.TipoSaldo 
	WHEN 0 THEN fpd.Valor - fpd.ValorPago
	ELSE fpd.ValorPago
END Valor,
fpd.TipoSaldo,
EXTRACT(YEAR FROM fpd.DataFatura) AS Ano,
Month(fpd.DataFatura) AS Mes,
fpd.IdPainel 
FROM fin_painel_dados fpd
LEFT JOIN fin_categorias fc ON fc.Id = fpd.IdCategoria");
            migrationBuilder.Sql(@"CREATE VIEW view_fin_painel_dados_cat AS
SELECT DISTINCT b.Categoria, b.Descricao, b.Ano, b.IdPainel FROM view_fin_painel_dados b");
            migrationBuilder.Sql(@"CREATE VIEW view_fin_painel_dados_rel AS
SELECT 
a.*,
CASE 
	WHEN jan.Id IS NOT NULL THEN jan.Id 
	ELSE ""
END JanId,
CASE 
	WHEN jan.Valor IS NOT NULL THEN jan.Valor 
	ELSE 0
END JanValor,
CASE 
	WHEN fev.Id IS NOT NULL THEN fev.Id 
	ELSE ""
END FevId,
CASE 
	WHEN fev.Valor IS NOT NULL THEN fev.Valor 
	ELSE 0
END FevValor,
CASE 
	WHEN mar.Id IS NOT NULL THEN mar.Id 
	ELSE ""
END MarId,
CASE 
	WHEN mar.Valor IS NOT NULL THEN mar.Valor 
	ELSE 0
END MarValor,
CASE 
	WHEN abr.Id IS NOT NULL THEN abr.Id 
	ELSE ""
END AbrId,
CASE 
	WHEN abr.Valor IS NOT NULL THEN abr.Valor 
	ELSE 0
END AbrValor,
CASE 
	WHEN mai.Id IS NOT NULL THEN mai.Id 
	ELSE ""
END MaiId,
CASE 
	WHEN mai.Valor IS NOT NULL THEN mai.Valor 
	ELSE 0
END MaiValor,
CASE 
	WHEN jun.Id IS NOT NULL THEN jun.Id 
	ELSE ""
END JunId,
CASE 
	WHEN jun.Valor IS NOT NULL THEN jun.Valor 
	ELSE 0
END JunValor,
CASE 
	WHEN jul.Id IS NOT NULL THEN jul.Id 
	ELSE ""
END JulId,
CASE 
	WHEN jul.Valor IS NOT NULL THEN jul.Valor 
	ELSE 0
END JulValor,
CASE 
	WHEN ago.Id IS NOT NULL THEN ago.Id 
	ELSE ""
END AgoId,
CASE 
	WHEN ago.Valor IS NOT NULL THEN ago.Valor 
	ELSE 0
END AgoValor,
CASE 
	WHEN setem.Id IS NOT NULL THEN setem.Id 
	ELSE ""
END SetId,
CASE 
	WHEN setem.Valor IS NOT NULL THEN setem.Valor 
	ELSE 0
END SetValor,
CASE 
	WHEN outu.Id IS NOT NULL THEN outu.Id 
	ELSE ""
END OutId,
CASE 
	WHEN outu.Valor IS NOT NULL THEN outu.Valor 
	ELSE 0
END OutValor,
CASE 
	WHEN nov.Id IS NOT NULL THEN nov.Id 
	ELSE ""
END NovId,
CASE 
	WHEN nov.Valor IS NOT NULL THEN nov.Valor 
	ELSE 0
END NovValor,
CASE 
	WHEN dez.Id IS NOT NULL THEN dez.Id 
	ELSE ""
END DezId,
CASE 
	WHEN dez.Valor IS NOT NULL THEN dez.Valor 
	ELSE 0
END DezValor
FROM view_fin_painel_dados_cat a
LEFT JOIN view_fin_painel_dados jan 
	ON jan.IdPainel = a.IdPainel 
		AND jan.Ano = a.Ano 
		AND jan.Categoria = a.Categoria 
		AND jan.Descricao = a.Descricao 
		AND jan.Mes = 1
LEFT JOIN view_fin_painel_dados fev 
	ON fev.IdPainel = a.IdPainel 
		AND fev.Ano = a.Ano 
		AND fev.Categoria = a.Categoria 
		AND fev.Descricao = a.Descricao 
		AND fev.Mes = 2
LEFT JOIN view_fin_painel_dados mar 
	ON mar.IdPainel = a.IdPainel 
		AND mar.Ano = a.Ano 
		AND mar.Categoria = a.Categoria 
		AND mar.Descricao = a.Descricao 
		AND mar.Mes = 3
LEFT JOIN view_fin_painel_dados abr 
	ON abr.IdPainel = a.IdPainel 
		AND abr.Ano = a.Ano 
		AND abr.Categoria = a.Categoria 
		AND abr.Descricao = a.Descricao 
		AND abr.Mes = 4
LEFT JOIN view_fin_painel_dados mai 
	ON mai.IdPainel = a.IdPainel 
		AND mai.Ano = a.Ano 
		AND mai.Categoria = a.Categoria 
		AND mai.Descricao = a.Descricao 
		AND mai.Mes = 5
LEFT JOIN view_fin_painel_dados jun 
	ON jun.IdPainel = a.IdPainel 
		AND jun.Ano = a.Ano 
		AND jun.Categoria = a.Categoria 
		AND jun.Descricao = a.Descricao 
		AND jun.Mes = 6
LEFT JOIN view_fin_painel_dados jul 
	ON jul.IdPainel = a.IdPainel 
		AND jul.Ano = a.Ano 
		AND jul.Categoria = a.Categoria 
		AND jul.Descricao = a.Descricao 
		AND jul.Mes = 7
LEFT JOIN view_fin_painel_dados ago 
	ON ago.IdPainel = a.IdPainel 
		AND ago.Ano = a.Ano 
		AND ago.Categoria = a.Categoria 
		AND ago.Descricao = a.Descricao 
		AND ago.Mes = 8
LEFT JOIN view_fin_painel_dados setem 
	ON setem.IdPainel = a.IdPainel 
		AND setem.Ano = a.Ano 
		AND setem.Categoria = a.Categoria 
		AND setem.Descricao = a.Descricao 
		AND setem.Mes = 9
LEFT JOIN view_fin_painel_dados outu 
	ON outu.IdPainel = a.IdPainel 
		AND outu.Ano = a.Ano 
		AND outu.Categoria = a.Categoria 
		AND outu.Descricao = a.Descricao 
		AND outu.Mes = 10
LEFT JOIN view_fin_painel_dados nov 
	ON nov.IdPainel = a.IdPainel 
		AND nov.Ano = a.Ano 
		AND nov.Categoria = a.Categoria 
		AND nov.Descricao = a.Descricao 
		AND nov.Mes = 11
LEFT JOIN view_fin_painel_dados dez 
	ON dez.IdPainel = a.IdPainel 
		AND dez.Ano = a.Ano 
		AND dez.Categoria = a.Categoria 
		AND dez.Descricao = a.Descricao 
		AND dez.Mes = 12");
			migrationBuilder.Sql(@"CREATE VIEW view_fin_painel_dados_rel_sum AS
SELECT
IdPainel,
Categoria,
Descricao,
Ano,
SUM(JanValor) JanValor,
SUM(FevValor) FevValor,
SUM(MarValor) MarValor,
SUM(AbrValor) AbrValor,
SUM(MaiValor) MaiValor,
SUM(JunValor) JunValor,
SUM(JulValor) JulValor,
SUM(AgoValor) AgoValor,
SUM(SetValor) SetValor,
SUM(OutValor) OutValor,
SUM(NovValor) NovValor,
SUM(DezValor) DezValor 
FROM view_fin_painel_dados_rel 
group by 
IdPainel,
Categoria,
Descricao,
Ano");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP VIEW view_fin_painel_dados");
            migrationBuilder.Sql("DROP VIEW view_fin_painel_dados_cat");
            migrationBuilder.Sql("DROP VIEW view_fin_painel_dados_rel");
            migrationBuilder.Sql("DROP VIEW view_fin_painel_dados_rel_sum");
        }
    }
}
