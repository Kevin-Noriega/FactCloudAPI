using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregaConfiguracionFactu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActividadEconomicaCIIU",
                table: "Negocios");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Negocios");

            migrationBuilder.DropColumn(
                name: "Nit",
                table: "Negocios");

            migrationBuilder.RenameColumn(
                name: "TipoPersona",
                table: "Negocios",
                newName: "CorreoRecepcionDian");

            migrationBuilder.RenameColumn(
                name: "NombreNegocio",
                table: "Negocios",
                newName: "CorreoElectronico");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Negocios",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RazonSocial",
                table: "Negocios",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Pais",
                table: "Negocios",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Negocios",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Departamento",
                table: "Negocios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ciudad",
                table: "Negocios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DatosFacturacionCompletos",
                table: "Negocios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NombreComercial",
                table: "Negocios",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroIdentificacionE",
                table: "Negocios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimerApellido",
                table: "Negocios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimerNombre",
                table: "Negocios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SegundoApellido",
                table: "Negocios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SegundoNombre",
                table: "Negocios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoDocumento",
                table: "Negocios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipoSujeto",
                table: "Negocios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PerfilesTributarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NegocioId = table.Column<int>(type: "int", nullable: false),
                    RegimenIvaCodigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ActividadEconomicaCIIU = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TributosJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsabilidadesFiscalesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilesTributarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilesTributarios_Negocios_NegocioId",
                        column: x => x.NegocioId,
                        principalTable: "Negocios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepresentantesLegales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NegocioId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoDocumento = table.Column<int>(type: "int", nullable: false),
                    NumeroIdentificacion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CiudadExpedicion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CiudadResidencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepresentantesLegales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepresentantesLegales_Negocios_NegocioId",
                        column: x => x.NegocioId,
                        principalTable: "Negocios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResolucionesDIAN_NegocioId_Activa",
                table: "ResolucionesDIAN",
                columns: new[] { "NegocioId", "Activa" });

            migrationBuilder.CreateIndex(
                name: "IX_PerfilesTributarios_NegocioId",
                table: "PerfilesTributarios",
                column: "NegocioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepresentantesLegales_NegocioId",
                table: "RepresentantesLegales",
                column: "NegocioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerfilesTributarios");

            migrationBuilder.DropTable(
                name: "RepresentantesLegales");

            migrationBuilder.DropIndex(
                name: "IX_ResolucionesDIAN_NegocioId_Activa",
                table: "ResolucionesDIAN");

            migrationBuilder.DropColumn(
                name: "DatosFacturacionCompletos",
                table: "Negocios");

            migrationBuilder.DropColumn(
                name: "NombreComercial",
                table: "Negocios");

            migrationBuilder.DropColumn(
                name: "NumeroIdentificacionE",
                table: "Negocios");

            migrationBuilder.DropColumn(
                name: "PrimerApellido",
                table: "Negocios");

            migrationBuilder.DropColumn(
                name: "PrimerNombre",
                table: "Negocios");

            migrationBuilder.DropColumn(
                name: "SegundoApellido",
                table: "Negocios");

            migrationBuilder.DropColumn(
                name: "SegundoNombre",
                table: "Negocios");

            migrationBuilder.DropColumn(
                name: "TipoDocumento",
                table: "Negocios");

            migrationBuilder.DropColumn(
                name: "TipoSujeto",
                table: "Negocios");

            migrationBuilder.RenameColumn(
                name: "CorreoRecepcionDian",
                table: "Negocios",
                newName: "TipoPersona");

            migrationBuilder.RenameColumn(
                name: "CorreoElectronico",
                table: "Negocios",
                newName: "NombreNegocio");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RazonSocial",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Pais",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Departamento",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ciudad",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "ActividadEconomicaCIIU",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nit",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
