using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FechaVigenciaFinal",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FechaVigenciaInicio",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsabilidadRut",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoPersona",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaVigenciaFinal",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaVigenciaInicio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ResponsabilidadRut",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TipoPersona",
                table: "Usuarios");
        }
    }
}
