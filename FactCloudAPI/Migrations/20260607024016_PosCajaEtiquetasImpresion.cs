using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NubeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class PosCajaEtiquetasImpresion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracionesImpresionPos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    MetodoImpresion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "navegador"),
                    ImpresoraDefecto = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TamanoPapel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Copias = table.Column<int>(type: "int", nullable: false),
                    ImpresionSimple = table.Column<bool>(type: "bit", nullable: false),
                    MargenSuperior = table.Column<int>(type: "int", nullable: false),
                    MargenInferior = table.Column<int>(type: "int", nullable: false),
                    MargenIzquierdo = table.Column<int>(type: "int", nullable: false),
                    MargenDerecho = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionesImpresionPos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracionesImpresionPos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EtiquetasPos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtiquetasPos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EtiquetasPos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosCajaPos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    NumeroComprobante = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosCajaPos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientosCajaPos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesImpresionPos_UsuarioId",
                table: "ConfiguracionesImpresionPos",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EtiquetasPos_UsuarioId",
                table: "EtiquetasPos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosCajaPos_UsuarioId_Fecha",
                table: "MovimientosCajaPos",
                columns: new[] { "UsuarioId", "Fecha" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionesImpresionPos");

            migrationBuilder.DropTable(
                name: "EtiquetasPos");

            migrationBuilder.DropTable(
                name: "MovimientosCajaPos");
        }
    }
}
