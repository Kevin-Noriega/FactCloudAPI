using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class codigoactualizado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoEAN",
                table: "Productos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoEAN",
                table: "Productos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
