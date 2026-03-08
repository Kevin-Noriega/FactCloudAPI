using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class Cambioenproducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseGravable",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "GravaINC",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "RetencionFuente",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "RetencionICA",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "RetencionIVA",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "TarifaINC",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "TarifaIVA",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "TipoImpuesto",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "ProductoExento",
                table: "Productos",
                newName: "IncluyeIVA");

            migrationBuilder.RenameColumn(
                name: "ProductoExcluido",
                table: "Productos",
                newName: "EsServicio");

            migrationBuilder.AlterColumn<string>(
                name: "UnidadMedida",
                table: "Productos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Unidad",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CantidadDisponible",
                table: "Productos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ImpuestoCargo",
                table: "Productos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Retencion",
                table: "Productos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CodigoInterno",
                table: "Productos",
                column: "CodigoInterno");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CodigoUNSPSC",
                table: "Productos",
                column: "CodigoUNSPSC");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_UsuarioId_Activo",
                table: "Productos",
                columns: new[] { "UsuarioId", "Activo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Productos_CodigoInterno",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_CodigoUNSPSC",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_UsuarioId_Activo",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "ImpuestoCargo",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Retencion",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "IncluyeIVA",
                table: "Productos",
                newName: "ProductoExento");

            migrationBuilder.RenameColumn(
                name: "EsServicio",
                table: "Productos",
                newName: "ProductoExcluido");

            migrationBuilder.AlterColumn<string>(
                name: "UnidadMedida",
                table: "Productos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Unidad");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CantidadDisponible",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseGravable",
                table: "Productos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GravaINC",
                table: "Productos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "RetencionFuente",
                table: "Productos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RetencionICA",
                table: "Productos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RetencionIVA",
                table: "Productos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TarifaINC",
                table: "Productos",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TarifaIVA",
                table: "Productos",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TipoImpuesto",
                table: "Productos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
