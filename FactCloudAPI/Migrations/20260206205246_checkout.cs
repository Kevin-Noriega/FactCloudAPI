using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class checkout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActividadEconomicaCIIU",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "AmbienteDIAN",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CiudadCodigo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CiudadNegocio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CorreoNegocio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DepartamentoCodigo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DepartamentoNegocio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DireccionNegocio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DvNitNegocio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaResolucionDIAN",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaVigenciaFinal",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaVigenciaInicio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "LogoNegocio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "NitNegocio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "NombreNegocio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "NumeroResolucionDIAN",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PrefijoAutorizadoDIAN",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "RangoNumeracionDesde",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "RangoNumeracionHasta",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "RegimenFiscal",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "RegimenTributario",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ResponsabilidadesRut",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "SoftwarePIN",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "SoftwareProveedor",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TelefonoNegocio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TipoPersona",
                table: "Usuarios");

            migrationBuilder.CreateTable(
                name: "Negocios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreNegocio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DvNit = table.Column<int>(type: "int", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Departamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoPersona = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActividadEconomicaCIIU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Negocios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Negocios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanesFacturacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PrecioMensual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioAnual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LimiteDocumentosMensual = table.Column<int>(type: "int", nullable: true),
                    LimiteUsuarios = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesFacturacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionesDian",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoftwareProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoftwarePIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrefijoAutorizadoDIAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroResolucionDIAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangoNumeracionDesde = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangoNumeracionHasta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmbienteDIAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaVigenciaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaVigenciaFinal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NegocioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionesDian", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracionesDian_Negocios_NegocioId",
                        column: x => x.NegocioId,
                        principalTable: "Negocios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuscripcionesFacturacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    PlanFacturacionId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentosUsados = table.Column<int>(type: "int", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuscripcionesFacturacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuscripcionesFacturacion_PlanesFacturacion_PlanFacturacionId",
                        column: x => x.PlanFacturacionId,
                        principalTable: "PlanesFacturacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuscripcionesFacturacion_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PlanesFacturacion",
                columns: new[] { "Id", "Activo", "Codigo", "LimiteDocumentosMensual", "LimiteUsuarios", "Nombre", "PrecioAnual", "PrecioMensual" },
                values: new object[,]
                {
                    { 1, true, "STARTER", 100, 1, "Starter", 70800m, 5900m },
                    { 2, true, "PAY_PER_USE", null, null, "Pago por Uso", 0m, 0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesDian_NegocioId",
                table: "ConfiguracionesDian",
                column: "NegocioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Negocios_UsuarioId",
                table: "Negocios",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuscripcionesFacturacion_PlanFacturacionId",
                table: "SuscripcionesFacturacion",
                column: "PlanFacturacionId");

            migrationBuilder.CreateIndex(
                name: "IX_SuscripcionesFacturacion_UsuarioId",
                table: "SuscripcionesFacturacion",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionesDian");

            migrationBuilder.DropTable(
                name: "SuscripcionesFacturacion");

            migrationBuilder.DropTable(
                name: "Negocios");

            migrationBuilder.DropTable(
                name: "PlanesFacturacion");

            migrationBuilder.AddColumn<string>(
                name: "ActividadEconomicaCIIU",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AmbienteDIAN",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CiudadCodigo",
                table: "Usuarios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CiudadNegocio",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "Usuarios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorreoNegocio",
                table: "Usuarios",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartamentoCodigo",
                table: "Usuarios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartamentoNegocio",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionNegocio",
                table: "Usuarios",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DvNitNegocio",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FechaResolucionDIAN",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

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
                name: "LogoNegocio",
                table: "Usuarios",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NitNegocio",
                table: "Usuarios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreNegocio",
                table: "Usuarios",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroResolucionDIAN",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "Usuarios",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrefijoAutorizadoDIAN",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RangoNumeracionDesde",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RangoNumeracionHasta",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegimenFiscal",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegimenTributario",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsabilidadesRut",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoftwarePIN",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoftwareProveedor",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelefonoNegocio",
                table: "Usuarios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoPersona",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
