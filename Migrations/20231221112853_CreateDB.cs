using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiBrechoRamires.Migrations
{
    /// <inheritdoc />
    public partial class CreateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendaProdutos_Produtos_ProdutoCodigo",
                table: "VendaProdutos");

            migrationBuilder.AlterColumn<string>(
                name: "ProdutoCodigo",
                table: "VendaProdutos",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_VendaProdutos_Produtos_ProdutoCodigo",
                table: "VendaProdutos",
                column: "ProdutoCodigo",
                principalTable: "Produtos",
                principalColumn: "Codigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendaProdutos_Produtos_ProdutoCodigo",
                table: "VendaProdutos");

            migrationBuilder.UpdateData(
                table: "VendaProdutos",
                keyColumn: "ProdutoCodigo",
                keyValue: null,
                column: "ProdutoCodigo",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProdutoCodigo",
                table: "VendaProdutos",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_VendaProdutos_Produtos_ProdutoCodigo",
                table: "VendaProdutos",
                column: "ProdutoCodigo",
                principalTable: "Produtos",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
