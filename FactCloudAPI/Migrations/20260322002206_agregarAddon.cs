using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class agregarAddon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuariosAddons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    AddonId = table.Column<int>(type: "int", nullable: false),
                    FechaContratacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosAddons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosAddons_Addons_AddonId",
                        column: x => x.AddonId,
                        principalTable: "Addons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuariosAddons_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Addons",
                columns: new[] { "Id", "Activo", "Color", "Descripcion", "Nombre", "Precio", "Tipo", "Unidad" },
                values: new object[,]
                {
                    { 1, true, "#1a73e8", "Agrega 150 documentos electrónicos adicionales a tu plan actual.", "Documentos extra (150)", 45000m, "Capacidad", "año" },
                    { 2, true, "#1a73e8", "Agrega 500 documentos electrónicos adicionales a tu plan actual.", "Documentos extra (500)", 120000m, "Capacidad", "año" },
                    { 3, true, "#0f6e56", "Permite que un usuario adicional acceda al sistema con tu cuenta.", "Usuario adicional", 60000m, "Usuarios", "año" },
                    { 4, true, "#7c3aed", "Accede a reportes detallados de ventas, clientes y tendencias de facturación.", "Reportes avanzados", 80000m, "Reportes", "año" },
                    { 5, true, "#b45309", "Atención prioritaria por chat y teléfono con tiempo de respuesta garantizado.", "Soporte prioritario", 50000m, "Soporte", "año" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAddons_AddonId",
                table: "UsuariosAddons",
                column: "AddonId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAddons_UsuarioId_Activo",
                table: "UsuariosAddons",
                columns: new[] { "UsuarioId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAddons_UsuarioId_AddonId",
                table: "UsuariosAddons",
                columns: new[] { "UsuarioId", "AddonId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuariosAddons");

            migrationBuilder.DeleteData(
                table: "Addons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Addons",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Addons",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Addons",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Addons",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
