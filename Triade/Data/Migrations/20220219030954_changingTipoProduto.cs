using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triade.Data.Migrations
{
    public partial class changingTipoProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSimple",
                table: "Produtos");

            migrationBuilder.AddColumn<int>(
                name: "ProdutoTipo",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProdutoTipo",
                table: "Produtos");

            migrationBuilder.AddColumn<bool>(
                name: "IsSimple",
                table: "Produtos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
