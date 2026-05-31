using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NubeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCodigoTributario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FactusRangoId",
                table: "Facturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CodigoMunicipio",
                table: "Clientes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoTributo",
                table: "Clientes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FactusRangoId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "CodigoMunicipio",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "CodigoTributo",
                table: "Clientes");
        }
    }
}
