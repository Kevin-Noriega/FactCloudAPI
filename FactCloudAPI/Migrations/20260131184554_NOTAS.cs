using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class NOTAS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotasCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroNota = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    NumeroFactura = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "devolucion"),
                    MotivoDIAN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaElaboracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CUFE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    XMLBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDescuentos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReteICA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNeto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente"),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotasCredito_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_NotasCredito_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotasCredito_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotasDebito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroNota = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    NumeroFactura = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MotivoDIAN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaElaboracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CUFE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    XMLBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDescuentos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReteICA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNeto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente"),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasDebito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotasDebito_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                });

            migrationBuilder.CreateTable(
                name: "DetalleNotaCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaCreditoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Unidad"),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubtotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleNotaCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleNotaCredito_NotasCredito_NotaCreditoId",
                        column: x => x.NotaCreditoId,
                        principalTable: "NotasCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleNotaCredito_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormasPagoNotaCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaCreditoId = table.Column<int>(type: "int", nullable: false),
                    Metodo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Efectivo"),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPagoNotaCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormasPagoNotaCredito_NotasCredito_NotaCreditoId",
                        column: x => x.NotaCreditoId,
                        principalTable: "NotasCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleNotaDebito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaDebitoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Unidad"),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubtotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleNotaDebito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleNotaDebito_NotasDebito_NotaDebitoId",
                        column: x => x.NotaDebitoId,
                        principalTable: "NotasDebito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleNotaDebito_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormasPagoNotaDebito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaDebitoId = table.Column<int>(type: "int", nullable: false),
                    Metodo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Efectivo"),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPagoNotaDebito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormasPagoNotaDebito_NotasDebito_NotaDebitoId",
                        column: x => x.NotaDebitoId,
                        principalTable: "NotasDebito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleNotaCredito_NotaCreditoId",
                table: "DetalleNotaCredito",
                column: "NotaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleNotaCredito_ProductoId",
                table: "DetalleNotaCredito",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleNotaDebito_NotaDebitoId",
                table: "DetalleNotaDebito",
                column: "NotaDebitoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleNotaDebito_ProductoId",
                table: "DetalleNotaDebito",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_FormasPagoNotaCredito_NotaCreditoId",
                table: "FormasPagoNotaCredito",
                column: "NotaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_FormasPagoNotaDebito_NotaDebitoId",
                table: "FormasPagoNotaDebito",
                column: "NotaDebitoId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_ClienteId",
                table: "NotasCredito",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_Estado",
                table: "NotasCredito",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_FacturaId",
                table: "NotasCredito",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_FechaElaboracion",
                table: "NotasCredito",
                column: "FechaElaboracion");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_NumeroNota",
                table: "NotasCredito",
                column: "NumeroNota");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_Tipo",
                table: "NotasCredito",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_UsuarioId",
                table: "NotasCredito",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_ClienteId",
                table: "NotasDebito",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_Estado",
                table: "NotasDebito",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_FacturaId",
                table: "NotasDebito",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_FechaElaboracion",
                table: "NotasDebito",
                column: "FechaElaboracion");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_NumeroNota",
                table: "NotasDebito",
                column: "NumeroNota");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_UsuarioId",
                table: "NotasDebito",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleNotaCredito");

            migrationBuilder.DropTable(
                name: "DetalleNotaDebito");

            migrationBuilder.DropTable(
                name: "FormasPagoNotaCredito");

            migrationBuilder.DropTable(
                name: "FormasPagoNotaDebito");

            migrationBuilder.DropTable(
                name: "NotasCredito");

            migrationBuilder.DropTable(
                name: "NotasDebito");
        }
    }
}
