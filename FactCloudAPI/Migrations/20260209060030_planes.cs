using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class planes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecioMensual",
                table: "PlanesFacturacion");

            migrationBuilder.RenameColumn(
                name: "LimiteDocumentosMensual",
                table: "PlanesFacturacion",
                newName: "LimiteDocumentosAnuales");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "PlanesFacturacion",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DescuentoActivo",
                table: "PlanesFacturacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DescuentoPorcentaje",
                table: "PlanesFacturacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Destacado",
                table: "PlanesFacturacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PlanFeature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanFacturacionId = table.Column<int>(type: "int", nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tooltip = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanFeature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanFeature_PlanesFacturacion_PlanFacturacionId",
                        column: x => x.PlanFacturacionId,
                        principalTable: "PlanesFacturacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PlanFeature",
                columns: new[] { "Id", "PlanFacturacionId", "Texto", "Tooltip" },
                values: new object[,]
                {
                    { 1, 1, "1 Usuario", "Cuenta individual para emprendedores que están empezando." },
                    { 2, 1, "30 Documentos anuales", "Emite hasta 30 facturas electrónicas al año." },
                    { 3, 1, "Funciones básicas", "Creación de facturas, gestión de clientes y productos. Reportes simples incluidos." },
                    { 4, 2, "1 Usuario", "Cuenta individual perfecta para emprendedores y negocios unipersonales.Acceso completo a todas las funciones." },
                    { 5, 2, "140 Documentos electrónicos al año", "Perfecto para negocios que emiten hasta 8 documentos diarios." },
                    { 6, 2, "Funciones básicas", "Creación de facturas, gestión de clientes, productos, notas débito y crédito. Reportes simples incluidos." }
                });

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Descripcion", "DescuentoActivo", "DescuentoPorcentaje", "Destacado", "LimiteDocumentosAnuales", "PrecioAnual" },
                values: new object[] { "Ideal para emprendedores iniciando", true, 15, false, 30, 135000m });

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Codigo", "Descripcion", "DescuentoActivo", "DescuentoPorcentaje", "Destacado", "LimiteDocumentosAnuales", "LimiteUsuarios", "Nombre", "PrecioAnual" },
                values: new object[] { "BASICO", "Para pequeños negocios en crecimiento", true, 10, false, 140, 1, "Básico", 3000000m });

            migrationBuilder.InsertData(
                table: "PlanesFacturacion",
                columns: new[] { "Id", "Activo", "Codigo", "Descripcion", "DescuentoActivo", "DescuentoPorcentaje", "Destacado", "DuracionMeses", "LimiteDocumentosAnuales", "LimiteUsuarios", "Nombre", "PrecioAnual" },
                values: new object[,]
                {
                    { 3, true, "PROFESIONAL", "Perfecto para PYMES establecidas", true, 10, false, 12, 540, 1, "Profesional", 770000m },
                    { 4, true, "EMPRESARIAL", "Solución completa para empresas grandes", true, 15, false, 12, 1550, 1, "Empresarial", 1300000m }
                });

            migrationBuilder.InsertData(
                table: "PlanFeature",
                columns: new[] { "Id", "PlanFacturacionId", "Texto", "Tooltip" },
                values: new object[,]
                {
                    { 7, 3, "1 Usuario", "Cuenta individual con acceso completo a todas las funcionalidades del sistema." },
                    { 8, 3, "540 Documentos electrónicos al año", "Ideal para PYMES que facturan de forma constante durante todo el año." },
                    { 9, 3, "Facturación electrónica DIAN", "Emisión de facturas electrónicas válidas ante la DIAN, cumpliendo la normativa vigente." },
                    { 10, 3, "Notas crédito y débito", "Corrección y ajustes de facturas mediante notas crédito y débito electrónicas." },
                    { 11, 3, "Gestión avanzada de clientes y productos", "Administra clientes, productos, precios e impuestos de forma organizada." },
                    { 12, 3, "Reportes y control de facturación", "Consulta reportes básicos de ventas, documentos emitidos y estado de facturación." },
                    { 13, 4, "1 Usuario", "Acceso completo al sistema con control total de la facturación empresarial." },
                    { 14, 4, "1550 Documentos electrónicos al año", "Pensado para empresas con alto volumen de facturación anual." },
                    { 15, 4, "Facturación electrónica DIAN", "Cumple con todos los requisitos exigidos por la DIAN para facturación electrónica." },
                    { 16, 4, "Notas crédito y débito ilimitadas", "Emite notas crédito y débito sin restricciones dentro del límite anual de documentos." },
                    { 17, 4, "Gestión completa de clientes y productos", "Control detallado de clientes, productos, impuestos y precios." },
                    { 18, 4, "Reportes administrativos", "Accede a reportes de ventas y facturación para control interno y contable." },
                    { 19, 4, "Soporte prioritario", "Atención prioritaria para resolución de dudas y soporte técnico." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanFeature_PlanFacturacionId",
                table: "PlanFeature",
                column: "PlanFacturacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanFeature");

            migrationBuilder.DeleteData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "PlanesFacturacion");

            migrationBuilder.DropColumn(
                name: "DescuentoActivo",
                table: "PlanesFacturacion");

            migrationBuilder.DropColumn(
                name: "DescuentoPorcentaje",
                table: "PlanesFacturacion");

            migrationBuilder.DropColumn(
                name: "Destacado",
                table: "PlanesFacturacion");

            migrationBuilder.RenameColumn(
                name: "LimiteDocumentosAnuales",
                table: "PlanesFacturacion",
                newName: "LimiteDocumentosMensual");

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioMensual",
                table: "PlanesFacturacion",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LimiteDocumentosMensual", "PrecioAnual", "PrecioMensual" },
                values: new object[] { 100, 70800m, 5900m });

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Codigo", "LimiteDocumentosMensual", "LimiteUsuarios", "Nombre", "PrecioAnual", "PrecioMensual" },
                values: new object[] { "PAY_PER_USE", null, null, "Pago por Uso", 0m, 0m });
        }
    }
}
