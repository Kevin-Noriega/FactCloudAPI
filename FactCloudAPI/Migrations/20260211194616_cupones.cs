using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class cupones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cupones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DescuentoPorcentaje = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxUsos = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    UsosCodigo = table.Column<int>(type: "int", nullable: false),
                    Expiracion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlanId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cupones_PlanesFacturacion_PlanId",
                        column: x => x.PlanId,
                        principalTable: "PlanesFacturacion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CuponUso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CuponId = table.Column<int>(type: "int", nullable: false),
                    SuscripcionId = table.Column<int>(type: "int", nullable: false),
                    UsadoAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DescuentoAplicado = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuponUso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuponUso_Cupones_CuponId",
                        column: x => x.CuponId,
                        principalTable: "Cupones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CuponUso_SuscripcionesFacturacion_SuscripcionId",
                        column: x => x.SuscripcionId,
                        principalTable: "SuscripcionesFacturacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cupones",
                columns: new[] { "Id", "Codigo", "DescuentoPorcentaje", "Expiracion", "IsActive", "MaxUsos", "PlanId", "UsosCodigo" },
                values: new object[,]
                {
                    { 1, "WELCOMEFC", 20m, null, false, 30, null, 0 },
                    { 2, "FACTCLOUDPRO", 30m, null, false, 30, 3, 0 },
                    { 3, "STARTEFC25", 12m, null, false, 20, 1, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cupones_PlanId",
                table: "Cupones",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_CuponUso_CuponId",
                table: "CuponUso",
                column: "CuponId");

            migrationBuilder.CreateIndex(
                name: "IX_CuponUso_SuscripcionId",
                table: "CuponUso",
                column: "SuscripcionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuponUso");

            migrationBuilder.DropTable(
                name: "Cupones");
        }
    }
}
