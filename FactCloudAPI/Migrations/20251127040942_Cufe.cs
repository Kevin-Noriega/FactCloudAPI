using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class Cufe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoRegimen",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "CUFE",
                table: "Facturas",
                newName: "Cufe");

            migrationBuilder.AddColumn<string>(
                name: "XmlBase64",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XmlBase64",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "Cufe",
                table: "Facturas",
                newName: "CUFE");

            migrationBuilder.AddColumn<string>(
                name: "TipoRegimen",
                table: "Usuarios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
