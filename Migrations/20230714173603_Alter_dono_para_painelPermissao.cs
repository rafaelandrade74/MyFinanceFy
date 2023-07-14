using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinanceFy.Migrations
{
    /// <inheritdoc />
    public partial class AlterdonoparapainelPermissao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dono",
                table: "fin_painel_usuarios");

            migrationBuilder.AddColumn<int>(
                name: "Permissao",
                table: "fin_painel_usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissao",
                table: "fin_painel_usuarios");

            migrationBuilder.AddColumn<bool>(
                name: "Dono",
                table: "fin_painel_usuarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
