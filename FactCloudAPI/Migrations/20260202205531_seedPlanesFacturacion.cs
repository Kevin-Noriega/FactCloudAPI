using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedPlanesFacturacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PlanesFacturacion",
                columns: new[] { "Id", "Activo", "Codigo", "LimiteDocumentosMensual", "LimiteUsuarios", "Nombre", "PrecioAnual", "PrecioMensual" },
                values: new object[,]
                {
                    { 1, true, "STARTER", 100, 1, "Starter", 70800m, 5900m },
                    { 2, true, "PAY_PER_USE", null, null, "Pago por Uso", 0m, 0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
