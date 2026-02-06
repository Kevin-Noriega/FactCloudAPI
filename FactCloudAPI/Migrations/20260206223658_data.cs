using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DuracionMeses",
                table: "PlanesFacturacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 1,
                column: "DuracionMeses",
                value: 12);

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 2,
                column: "DuracionMeses",
                value: 12);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DuracionMeses",
                table: "PlanesFacturacion");
        }
    }
}
