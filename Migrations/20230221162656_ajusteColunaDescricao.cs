using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinanceFy.Migrations
{
    /// <inheritdoc />
    public partial class ajusteColunaDescricao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "fin_painel_dados",
                newName: "Observacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "fin_painel_dados",
                newName: "Nome");
        }
    }
}
