using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NubeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class Addpios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncluyePOS",
                table: "PlanesFacturacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 1,
                column: "IncluyePOS",
                value: false);

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 2,
                column: "IncluyePOS",
                value: false);

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 3,
                column: "IncluyePOS",
                value: false);

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 4,
                column: "IncluyePOS",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncluyePOS",
                table: "PlanesFacturacion");
        }
    }
}
