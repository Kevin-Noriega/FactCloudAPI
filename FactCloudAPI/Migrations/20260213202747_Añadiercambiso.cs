using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class Añadiercambiso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrosPendientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransaccionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DatosRegistro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatosNegocio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatosPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "PENDING"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotasError = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosPendientes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPendientes_Email",
                table: "RegistrosPendientes",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPendientes_Estado",
                table: "RegistrosPendientes",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPendientes_TransaccionId",
                table: "RegistrosPendientes",
                column: "TransaccionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrosPendientes");
        }
    }
}
