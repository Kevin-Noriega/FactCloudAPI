using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class Actualizarvariables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoRegimen",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "TipoResponsabilidad",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "MunicipioCodigo",
                table: "Clientes",
                newName: "CiudadCodigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CiudadCodigo",
                table: "Clientes",
                newName: "MunicipioCodigo");

            migrationBuilder.AddColumn<string>(
                name: "TipoRegimen",
                table: "Clientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoResponsabilidad",
                table: "Clientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
