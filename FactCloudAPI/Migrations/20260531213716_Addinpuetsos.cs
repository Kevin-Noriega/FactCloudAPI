using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NubeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class Addinpuetsos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseMinimaCompras",
                table: "Impuestos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseMinimaVentas",
                table: "Impuestos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoFormatoExogena",
                table: "Impuestos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConceptoRetencionDIAN",
                table: "Impuestos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsBienExcluido",
                table: "Impuestos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EsBienExento",
                table: "Impuestos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GeneraInformacionExogena",
                table: "Impuestos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TipoBaseMinimaCompras",
                table: "Impuestos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoBaseMinimaVentas",
                table: "Impuestos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "VigenteDesde",
                table: "Impuestos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VigenteHasta",
                table: "Impuestos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "BaseMinimaCompras", "BaseMinimaVentas", "CodigoFormatoExogena", "ConceptoRetencionDIAN", "EsBienExcluido", "EsBienExento", "GeneraInformacionExogena", "TipoBaseMinimaCompras", "TipoBaseMinimaVentas", "VigenteDesde", "VigenteHasta" },
                values: new object[] { null, null, null, null, false, false, false, "Pesos", "Pesos", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.CreateIndex(
                name: "IX_Impuesto_Tipo_Tenant",
                table: "Impuestos",
                columns: new[] { "TipoImpuesto", "UsuarioId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Impuesto_Tipo_Tenant",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "BaseMinimaCompras",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "BaseMinimaVentas",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "CodigoFormatoExogena",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "ConceptoRetencionDIAN",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "EsBienExcluido",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "EsBienExento",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "GeneraInformacionExogena",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "TipoBaseMinimaCompras",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "TipoBaseMinimaVentas",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "VigenteDesde",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "VigenteHasta",
                table: "Impuestos");
        }
    }
}
