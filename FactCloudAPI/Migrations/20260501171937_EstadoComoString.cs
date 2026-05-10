using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class EstadoComoString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TarifaICA",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "TarifaINC",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "TarifaIVA",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "ValorICA",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "ValorINC",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "ValorIVA",
                table: "DetalleFacturas");

            migrationBuilder.AddColumn<int>(
                name: "CuentaContableId",
                table: "Impuestos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaContableId1",
                table: "Impuestos",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "CuentaContableId", "CuentaContableId1" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_CuentaContableId",
                table: "Impuestos",
                column: "CuentaContableId");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_CuentaContableId1",
                table: "Impuestos",
                column: "CuentaContableId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Impuestos_CuentasContables_CuentaContableId",
                table: "Impuestos",
                column: "CuentaContableId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Impuestos_CuentasContables_CuentaContableId1",
                table: "Impuestos",
                column: "CuentaContableId1",
                principalTable: "CuentasContables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Impuestos_CuentasContables_CuentaContableId",
                table: "Impuestos");

            migrationBuilder.DropForeignKey(
                name: "FK_Impuestos_CuentasContables_CuentaContableId1",
                table: "Impuestos");

            migrationBuilder.DropIndex(
                name: "IX_Impuestos_CuentaContableId",
                table: "Impuestos");

            migrationBuilder.DropIndex(
                name: "IX_Impuestos_CuentaContableId1",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "CuentaContableId",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "CuentaContableId1",
                table: "Impuestos");

            migrationBuilder.AddColumn<decimal>(
                name: "TarifaICA",
                table: "DetalleFacturas",
                type: "decimal(6,4)",
                precision: 6,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TarifaINC",
                table: "DetalleFacturas",
                type: "decimal(6,4)",
                precision: 6,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TarifaIVA",
                table: "DetalleFacturas",
                type: "decimal(6,4)",
                precision: 6,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorICA",
                table: "DetalleFacturas",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorINC",
                table: "DetalleFacturas",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorIVA",
                table: "DetalleFacturas",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
