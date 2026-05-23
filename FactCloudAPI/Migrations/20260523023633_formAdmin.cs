using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NubeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class formAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegistrosPendientes_Email",
                table: "RegistrosPendientes");

            migrationBuilder.AlterColumn<string>(
                name: "Pais",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "CO",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nit",
                table: "Negocios",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "DatosFacturacionCompletos",
                table: "Negocios",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "SoftwareProveedor",
                table: "ConfiguracionesDian",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoftwarePIN",
                table: "ConfiguracionesDian",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RangoNumeracionHasta",
                table: "ConfiguracionesDian",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RangoNumeracionDesde",
                table: "ConfiguracionesDian",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrefijoAutorizadoDIAN",
                table: "ConfiguracionesDian",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeroResolucionDIAN",
                table: "ConfiguracionesDian",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AmbienteDIAN",
                table: "ConfiguracionesDian",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "Habilitacion",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResolucionesDIAN_NegocioId_Activa",
                table: "ResolucionesDIAN",
                columns: new[] { "NegocioId", "Activa" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPendientes_Email",
                table: "RegistrosPendientes",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Negocios_Nit",
                table: "Negocios",
                column: "Nit",
                unique: true,
                filter: "[Nit] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ResolucionesDIAN_NegocioId_Activa",
                table: "ResolucionesDIAN");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosPendientes_Email",
                table: "RegistrosPendientes");

            migrationBuilder.DropIndex(
                name: "IX_Negocios_Nit",
                table: "Negocios");



            migrationBuilder.AlterColumn<string>(
                name: "Pais",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "CO");

            migrationBuilder.AlterColumn<string>(
                name: "Nit",
                table: "Negocios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "DatosFacturacionCompletos",
                table: "Negocios",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "SoftwareProveedor",
                table: "ConfiguracionesDian",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoftwarePIN",
                table: "ConfiguracionesDian",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RangoNumeracionHasta",
                table: "ConfiguracionesDian",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RangoNumeracionDesde",
                table: "ConfiguracionesDian",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrefijoAutorizadoDIAN",
                table: "ConfiguracionesDian",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeroResolucionDIAN",
                table: "ConfiguracionesDian",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AmbienteDIAN",
                table: "ConfiguracionesDian",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "Habilitacion");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPendientes_Email",
                table: "RegistrosPendientes",
                column: "Correo");
        }
    }
}
