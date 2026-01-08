using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarUsuario2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsabilidadRut",
                table: "Usuarios");

            migrationBuilder.AddColumn<string>(
                name: "ResponsabilidadesRut",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsabilidadesRut",
                table: "Usuarios");

            migrationBuilder.AddColumn<string>(
                name: "ResponsabilidadRut",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
