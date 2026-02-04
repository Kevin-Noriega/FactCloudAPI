using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class documento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentosSoporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prefijo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Consecutivo = table.Column<int>(type: "int", nullable: false),
                    CUDS = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProveedorNombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProveedorNit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProveedorTipoIdentificacion = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ProveedorDireccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProveedorCiudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProveedorDepartamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProveedorEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProveedorTelefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EstadoDian = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MensajeDian = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaRespuestaDian = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumeroRespuestaDian = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RutaXML = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RutaPDF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosSoporte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentosSoporte_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosSoporte_UsuarioId",
                table: "DocumentosSoporte",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentosSoporte");
        }
    }
}
