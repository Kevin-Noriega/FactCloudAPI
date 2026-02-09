using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class nuevo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 2,
                column: "PrecioAnual",
                value: 300000m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 2,
                column: "PrecioAnual",
                value: 3000000m);
        }
    }
}
