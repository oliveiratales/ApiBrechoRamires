using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiBrechoRamires.Migrations
{
    /// <inheritdoc />
    public partial class Teste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendaProdutos_Produtos_ProdutoCodigo",
                table: "VendaProdutos");

            migrationBuilder.AddForeignKey(
                name: "FK_VendaProdutos_Produtos_ProdutoCodigo",
                table: "VendaProdutos",
                column: "ProdutoCodigo",
                principalTable: "Produtos",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendaProdutos_Produtos_ProdutoCodigo",
                table: "VendaProdutos");

            migrationBuilder.AddForeignKey(
                name: "FK_VendaProdutos_Produtos_ProdutoCodigo",
                table: "VendaProdutos",
                column: "ProdutoCodigo",
                principalTable: "Produtos",
                principalColumn: "Codigo");
        }
    }
}
