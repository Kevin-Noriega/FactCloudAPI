using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NubeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class pos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AplicaIVAExcluido",
                table: "Impuestos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AplicaIVAExento",
                table: "Impuestos",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                name: "CodigoConceptoExogena",
                table: "Impuestos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConceptoRetencionDIAN",
                table: "Impuestos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GeneraInformacionExogena",
                table: "Impuestos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Impuestos",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "AplicaIVAExcluido", "AplicaIVAExento", "BaseMinimaCompras", "BaseMinimaVentas", "CodigoConceptoExogena", "ConceptoRetencionDIAN", "GeneraInformacionExogena" },
                values: new object[] { false, false, null, null, null, null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AplicaIVAExcluido",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "AplicaIVAExento",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "BaseMinimaCompras",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "BaseMinimaVentas",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "CodigoConceptoExogena",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "ConceptoRetencionDIAN",
                table: "Impuestos");

            migrationBuilder.DropColumn(
                name: "GeneraInformacionExogena",
                table: "Impuestos");
        }
    }
}
