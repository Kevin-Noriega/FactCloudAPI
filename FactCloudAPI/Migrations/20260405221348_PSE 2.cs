using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class PSE2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegistrosPendientes_Email",
                table: "RegistrosPendientes");

            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "RegistrosPendientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "RegistrosPendientes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorreoNegocio",
                table: "RegistrosPendientes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Departamento",
                table: "RegistrosPendientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "RegistrosPendientes",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DvNit",
                table: "RegistrosPendientes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAprobacion",
                table: "RegistrosPendientes",
                type: "datetime2",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nit",
                table: "RegistrosPendientes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "RegistrosPendientes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreNegocio",
                table: "RegistrosPendientes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroIdentificacion",
                table: "RegistrosPendientes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "RegistrosPendientes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PlanFacturacionId",
                table: "RegistrosPendientes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "RegistrosPendientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TelefonoNegocio",
                table: "RegistrosPendientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoIdentificacion",
                table: "RegistrosPendientes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WompiReference",
                table: "RegistrosPendientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "RegimenTributario",
                table: "Clientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPendientes_Email",
                table: "RegistrosPendientes",
                column: "Correo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegistrosPendientes_Email",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "CorreoNegocio",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "Departamento",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "DvNit",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "FechaAprobacion",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "Nit",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "NombreNegocio",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "NumeroIdentificacion",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "PlanFacturacionId",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "TelefonoNegocio",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "TipoIdentificacion",
                table: "RegistrosPendientes");

            migrationBuilder.DropColumn(
                name: "WompiReference",
                table: "RegistrosPendientes");

            migrationBuilder.AlterColumn<string>(
                name: "RegimenTributario",
                table: "Clientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPendientes_Email",
                table: "RegistrosPendientes",
                column: "Email");
        }
    }
}
