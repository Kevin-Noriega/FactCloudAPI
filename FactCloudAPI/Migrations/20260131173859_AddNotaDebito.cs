using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNotaDebito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotasDebito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaElaboracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    VendedorId = table.Column<int>(type: "int", nullable: false),
                    TotalBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDescuentos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNeto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArchivoAdjunto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasDebito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotasDebito_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotasDebito_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotasDebito_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotasDebito_Usuarios_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotaDebitoDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaDebitoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImpuestoCargo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImpuestoRetencion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaDebitoDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotaDebitoDetalle_NotasDebito_NotaDebitoId",
                        column: x => x.NotaDebitoId,
                        principalTable: "NotasDebito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotaDebitoDetalle_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotaDebitoPago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaDebitoId = table.Column<int>(type: "int", nullable: false),
                    FormaPago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaDebitoPago", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotaDebitoPago_NotasDebito_NotaDebitoId",
                        column: x => x.NotaDebitoId,
                        principalTable: "NotasDebito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotaDebitoDetalle_NotaDebitoId",
                table: "NotaDebitoDetalle",
                column: "NotaDebitoId");

            migrationBuilder.CreateIndex(
                name: "IX_NotaDebitoDetalle_ProductoId",
                table: "NotaDebitoDetalle",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_NotaDebitoPago_NotaDebitoId",
                table: "NotaDebitoPago",
                column: "NotaDebitoId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_ClienteId",
                table: "NotasDebito",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_FacturaId",
                table: "NotasDebito",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_UsuarioId",
                table: "NotasDebito",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_VendedorId",
                table: "NotasDebito",
                column: "VendedorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotaDebitoDetalle");

            migrationBuilder.DropTable(
                name: "NotaDebitoPago");

            migrationBuilder.DropTable(
                name: "NotasDebito");
        }
    }
}
