using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NubeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class PlanesYSistemaPOS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncluyeContabilidad",
                table: "PlanesFacturacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncluyeInventario",
                table: "PlanesFacturacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncluyeNomina",
                table: "PlanesFacturacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncluyeSucursales",
                table: "PlanesFacturacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "PlanesFacturacion",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 3,
                column: "Texto",
                value: "Facturación electrónica DIAN");

            migrationBuilder.UpdateData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Texto", "Tooltip" },
                values: new object[] { "2 Usuarios", "Hasta dos usuarios para tu negocio." });

            migrationBuilder.UpdateData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 6,
                column: "Texto",
                value: "Facturación electrónica DIAN");

            migrationBuilder.UpdateData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Texto", "Tooltip" },
                values: new object[] { "5 Usuarios", "Hasta cinco usuarios con acceso al sistema." });

            migrationBuilder.UpdateData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Texto", "Tooltip" },
                values: new object[] { "15 Usuarios", "Hasta quince usuarios para equipos grandes." });

            migrationBuilder.InsertData(
                table: "PlanFeature",
                columns: new[] { "Id", "PlanFacturacionId", "Texto", "Tooltip" },
                values: new object[,]
                {
                    { 45, 2, "Control de inventario", "Administra existencias y stock de tus productos." },
                    { 46, 3, "Control de inventario", "Descuento automático de stock y control de existencias." },
                    { 47, 4, "Control de inventario", "Inventario por bodega y existencias en tiempo real." },
                    { 48, 4, "Nómina electrónica", "Liquidación y emisión de nómina electrónica ante la DIAN." },
                    { 49, 4, "Contabilidad integrada", "Causación contable automática de tus documentos." },
                    { 50, 4, "Multi-sucursal", "Administra varias sucursales desde una sola cuenta." }
                });

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IncluyeContabilidad", "IncluyeInventario", "IncluyeNomina", "IncluyeSucursales", "Tipo" },
                values: new object[] { false, false, false, false, "FACTURACION" });

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IncluyeContabilidad", "IncluyeInventario", "IncluyeNomina", "IncluyeSucursales", "LimiteUsuarios", "Tipo" },
                values: new object[] { false, true, false, false, 2, "FACTURACION" });

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Destacado", "IncluyeContabilidad", "IncluyeInventario", "IncluyeNomina", "IncluyeSucursales", "LimiteUsuarios", "Tipo" },
                values: new object[] { true, false, true, false, false, 5, "FACTURACION" });

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IncluyeContabilidad", "IncluyeInventario", "IncluyeNomina", "IncluyeSucursales", "LimiteUsuarios", "Tipo" },
                values: new object[] { true, true, true, true, 15, "FACTURACION" });

            migrationBuilder.InsertData(
                table: "PlanesFacturacion",
                columns: new[] { "Id", "Activo", "Codigo", "Descripcion", "DescuentoActivo", "DescuentoPorcentaje", "Destacado", "DuracionMeses", "IncluyeContabilidad", "IncluyeInventario", "IncluyeNomina", "IncluyePOS", "IncluyeSucursales", "LimiteDocumentosAnuales", "LimiteUsuarios", "Nombre", "PrecioAnual", "Tipo" },
                values: new object[,]
                {
                    { 5, true, "POS_ESENCIAL", "Punto de venta para empezar a vender desde el mostrador", false, null, true, 12, false, true, false, true, false, 240, 1, "Sistema POS Esencial", 345000m, "POS" },
                    { 6, true, "POS_INICIO", "Para negocios con mayor volumen de ventas", true, 10, false, 12, false, true, false, true, false, null, 2, "Sistema POS Inicio", 585900m, "POS" },
                    { 7, true, "POS_AVANZADO", "Operación profesional con inventario y reportes avanzados", true, 15, false, 12, false, true, false, true, true, null, 5, "Sistema POS Avanzado", 899900m, "POS" }
                });

            migrationBuilder.InsertData(
                table: "PlanFeature",
                columns: new[] { "Id", "PlanFacturacionId", "Texto", "Tooltip" },
                values: new object[,]
                {
                    { 20, 5, "Cumplimiento normativo DIAN", "Documentos POS válidos ante la DIAN." },
                    { 21, 5, "240 Facturas electrónicas anuales desde POS", "Hasta 240 documentos electrónicos al año desde el punto de venta." },
                    { 22, 5, "1 Caja registradora", "Una terminal de venta para atender en mostrador." },
                    { 23, 5, "Control de inventario", "Descuento automático de stock por cada venta." },
                    { 24, 5, "Reportes de ventas diarios", "Total de ventas del día y productos más vendidos." },
                    { 25, 6, "Cumplimiento normativo DIAN", "Documentos POS válidos ante la DIAN." },
                    { 26, 6, "Facturas electrónicas desde POS ilimitadas", "Sin límite de documentos electrónicos desde el POS." },
                    { 27, 6, "2 Cajas registradoras", "Atiende en dos terminales simultáneas." },
                    { 28, 6, "Inventario con alertas de stock mínimo", "Recibe avisos cuando un producto esté por agotarse." },
                    { 29, 6, "Cierre de caja por turno", "Controla el dinero por turno con apertura y cierre de caja." },
                    { 31, 7, "Cumplimiento normativo DIAN", "Documentos POS válidos ante la DIAN." },
                    { 32, 7, "Facturas electrónicas desde POS ilimitadas", "Sin límite de documentos electrónicos desde el POS." },
                    { 33, 7, "Cajas registradoras ilimitadas", "Sin límite en el número de terminales de venta." },
                    { 34, 7, "Inventario avanzado por bodega", "Controla existencias en varias bodegas o ubicaciones." },
                    { 35, 7, "Arqueo y reportes avanzados", "Cuadre de caja detallado y reportes exportables." },
                    { 36, 7, "Soporte prioritario 24/7", "Atención preferencial todos los días a cualquier hora." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "IncluyeContabilidad",
                table: "PlanesFacturacion");

            migrationBuilder.DropColumn(
                name: "IncluyeInventario",
                table: "PlanesFacturacion");

            migrationBuilder.DropColumn(
                name: "IncluyeNomina",
                table: "PlanesFacturacion");

            migrationBuilder.DropColumn(
                name: "IncluyeSucursales",
                table: "PlanesFacturacion");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "PlanesFacturacion");

            migrationBuilder.UpdateData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 3,
                column: "Texto",
                value: "Funciones básicas");

            migrationBuilder.UpdateData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Texto", "Tooltip" },
                values: new object[] { "1 Usuario", "Cuenta individual perfecta para emprendedores y negocios unipersonales." });

            migrationBuilder.UpdateData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 6,
                column: "Texto",
                value: "Funciones básicas");

            migrationBuilder.UpdateData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Texto", "Tooltip" },
                values: new object[] { "1 Usuario", "Cuenta individual con acceso completo a todas las funcionalidades." });

            migrationBuilder.UpdateData(
                table: "PlanFeature",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Texto", "Tooltip" },
                values: new object[] { "1 Usuario", "Acceso completo al sistema con control total de la facturación empresarial." });

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 2,
                column: "LimiteUsuarios",
                value: 1);

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Destacado", "LimiteUsuarios" },
                values: new object[] { false, 1 });

            migrationBuilder.UpdateData(
                table: "PlanesFacturacion",
                keyColumn: "Id",
                keyValue: 4,
                column: "LimiteUsuarios",
                value: 1);
        }
    }
}
