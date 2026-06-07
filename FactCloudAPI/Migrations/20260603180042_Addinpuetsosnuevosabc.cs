using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NubeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class Addinpuetsosnuevosabc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Impuesto_Tipo_Tenant",
                table: "Impuestos");

            migrationBuilder.AddColumn<string>(
                name: "NumeroFactus",
                table: "Facturas",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FacturasFormasPago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    MetodoPagoCodigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturasFormasPago", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacturasFormasPago_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 16,
                column: "CodigoTributoDIAN",
                value: "04");

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 17,
                column: "CodigoTributoDIAN",
                value: "04");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasFormasPago_FacturaId",
                table: "FacturasFormasPago",
                column: "FacturaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacturasFormasPago");

            migrationBuilder.DropColumn(
                name: "NumeroFactus",
                table: "Facturas");

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 16,
                column: "CodigoTributoDIAN",
                value: "02");

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 17,
                column: "CodigoTributoDIAN",
                value: "02");

            migrationBuilder.CreateIndex(
                name: "IX_Impuesto_Tipo_Tenant",
                table: "Impuestos",
                columns: new[] { "TipoImpuesto", "UsuarioId" });
        }
    }
}
