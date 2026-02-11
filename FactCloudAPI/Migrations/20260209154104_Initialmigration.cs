using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanesFacturacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrecioAnual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Destacado = table.Column<bool>(type: "bit", nullable: false),
                    DescuentoPorcentaje = table.Column<int>(type: "int", nullable: true),
                    DescuentoActivo = table.Column<bool>(type: "bit", nullable: false),
                    LimiteDocumentosAnuales = table.Column<int>(type: "int", nullable: true),
                    LimiteUsuarios = table.Column<int>(type: "int", nullable: true),
                    DuracionMeses = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesFacturacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoIdentificacion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NumeroIdentificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContrasenaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaDesactivacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NombreComercial = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TipoIdentificacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumeroIdentificacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DigitoVerificacion = table.Column<int>(type: "int", nullable: true),
                    TipoPersona = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegimenTributario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Departamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CiudadCodigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DepartamentoCodigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ActividadEconomicaCIIU = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RegimenFiscal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RetenedorIVA = table.Column<bool>(type: "bit", nullable: false),
                    RetenedorICA = table.Column<bool>(type: "bit", nullable: false),
                    RetenedorRenta = table.Column<bool>(type: "bit", nullable: false),
                    AutoretenedorRenta = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentosSoporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prefijo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Consecutivo = table.Column<int>(type: "int", nullable: false),
                    CUDS = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProveedorNombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProveedorNit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProveedorTipoIdentificacion = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ProveedorDireccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProveedorCiudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProveedorDepartamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProveedorEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProveedorTelefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EstadoDian = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MensajeDian = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaRespuestaDian = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumeroRespuestaDian = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RutaXML = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RutaPDF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosSoporte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentosSoporte_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Negocios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreNegocio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DvNit = table.Column<int>(type: "int", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Departamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoPersona = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActividadEconomicaCIIU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Negocios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Negocios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CodigoInterno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CodigoUNSPSC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnidadMedida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TipoImpuesto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TarifaIVA = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ProductoExcluido = table.Column<bool>(type: "bit", nullable: false),
                    ProductoExento = table.Column<bool>(type: "bit", nullable: false),
                    GravaINC = table.Column<bool>(type: "bit", nullable: false),
                    TarifaINC = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CantidadDisponible = table.Column<int>(type: "int", nullable: false),
                    CantidadMinima = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CodigoBarras = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TipoProducto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BaseGravable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RetencionFuente = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RetencionIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RetencionICA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Usado = table.Column<bool>(type: "bit", nullable: false),
                    Revocado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuscripcionesFacturacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    PlanFacturacionId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentosUsados = table.Column<int>(type: "int", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuscripcionesFacturacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuscripcionesFacturacion_PlanesFacturacion_PlanFacturacionId",
                        column: x => x.PlanFacturacionId,
                        principalTable: "PlanesFacturacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuscripcionesFacturacion_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    NumeroFactura = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prefijo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraEmision = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cufe = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QRCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalINC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDescuentos = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalRetenciones = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalFactura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FormaPago = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MedioPago = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiasCredito = table.Column<int>(type: "int", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EnviadaDIAN = table.Column<bool>(type: "bit", nullable: false),
                    FechaLimiteEnvioDIAN = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEnvioDIAN = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RespuestaDIAN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnviadaCliente = table.Column<bool>(type: "bit", nullable: false),
                    FechaEnvioCliente = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MontoPagado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: true),
                    XmlBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facturas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facturas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionesDian",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoftwareProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoftwarePIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrefijoAutorizadoDIAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroResolucionDIAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangoNumeracionDesde = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangoNumeracionHasta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmbienteDIAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaVigenciaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaVigenciaFinal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NegocioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionesDian", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracionesDian_Negocios_NegocioId",
                        column: x => x.NegocioId,
                        principalTable: "Negocios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleFacturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ValorDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SubtotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaIVA = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ValorIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaINC = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ValorINC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductoId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleFacturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleFacturas_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleFacturas_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleFacturas_Productos_ProductoId1",
                        column: x => x.ProductoId1,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NotasCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroNota = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    NumeroFactura = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "devolucion"),
                    MotivoDIAN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaElaboracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CUFE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    XMLBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDescuentos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReteICA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNeto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente"),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotasCredito_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_NotasCredito_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotasCredito_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotasDebito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroNota = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    NumeroFactura = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MotivoDIAN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaElaboracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CUFE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    XMLBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDescuentos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReteICA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNeto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente"),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ClienteId1 = table.Column<int>(type: "int", nullable: true),
                    FacturaId1 = table.Column<int>(type: "int", nullable: true),
                    UsuarioId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasDebito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotasDebito_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_NotasDebito_Clientes_ClienteId1",
                        column: x => x.ClienteId1,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotasDebito_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotasDebito_Facturas_FacturaId1",
                        column: x => x.FacturaId1,
                        principalTable: "Facturas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotasDebito_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotasDebito_Usuarios_UsuarioId1",
                        column: x => x.UsuarioId1,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DetalleNotaCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaCreditoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Unidad"),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubtotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleNotaCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleNotaCredito_NotasCredito_NotaCreditoId",
                        column: x => x.NotaCreditoId,
                        principalTable: "NotasCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleNotaCredito_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormasPagoNotaCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaCreditoId = table.Column<int>(type: "int", nullable: false),
                    Metodo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Efectivo"),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPagoNotaCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormasPagoNotaCredito_NotasCredito_NotaCreditoId",
                        column: x => x.NotaCreditoId,
                        principalTable: "NotasCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleNotaDebito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaDebitoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Unidad"),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubtotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorINC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalLinea = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleNotaDebito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleNotaDebito_NotasDebito_NotaDebitoId",
                        column: x => x.NotaDebitoId,
                        principalTable: "NotasDebito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleNotaDebito_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormasPagoNotaDebito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaDebitoId = table.Column<int>(type: "int", nullable: false),
                    Metodo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Efectivo"),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPagoNotaDebito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormasPagoNotaDebito_NotasDebito_NotaDebitoId",
                        column: x => x.NotaDebitoId,
                        principalTable: "NotasDebito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PlanesFacturacion",
                columns: new[] { "Id", "Activo", "Codigo", "Descripcion", "DescuentoActivo", "DescuentoPorcentaje", "Destacado", "DuracionMeses", "LimiteDocumentosAnuales", "LimiteUsuarios", "Nombre", "PrecioAnual" },
                values: new object[,]
                {
                    { 1, true, "STARTER", "Ideal para emprendedores iniciando", true, 15, false, 12, 30, 1, "Starter", 135000m },
                    { 2, true, "BASICO", "Para pequeños negocios en crecimiento", true, 10, false, 12, 140, 1, "Básico", 300000m },
                    { 3, true, "PROFESIONAL", "Perfecto para PYMES establecidas", true, 10, false, 12, 540, 1, "Profesional", 770000m },
                    { 4, true, "EMPRESARIAL", "Solución completa para empresas grandes", true, 15, false, 12, 1550, 1, "Empresarial", 1300000m }
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
                    { 6, 2, "Funciones básicas", "Creación de facturas, gestión de clientes, productos, notas débito y crédito. Reportes simples incluidos." },
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
                name: "IX_Clientes_UsuarioId",
                table: "Clientes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesDian_NegocioId",
                table: "ConfiguracionesDian",
                column: "NegocioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_FacturaId",
                table: "DetalleFacturas",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_ProductoId",
                table: "DetalleFacturas",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_ProductoId1",
                table: "DetalleFacturas",
                column: "ProductoId1");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleNotaCredito_NotaCreditoId",
                table: "DetalleNotaCredito",
                column: "NotaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleNotaCredito_ProductoId",
                table: "DetalleNotaCredito",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleNotaDebito_NotaDebitoId",
                table: "DetalleNotaDebito",
                column: "NotaDebitoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleNotaDebito_ProductoId",
                table: "DetalleNotaDebito",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosSoporte_UsuarioId",
                table: "DocumentosSoporte",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_ClienteId",
                table: "Facturas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UsuarioId",
                table: "Facturas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_FormasPagoNotaCredito_NotaCreditoId",
                table: "FormasPagoNotaCredito",
                column: "NotaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_FormasPagoNotaDebito_NotaDebitoId",
                table: "FormasPagoNotaDebito",
                column: "NotaDebitoId");

            migrationBuilder.CreateIndex(
                name: "IX_Negocios_UsuarioId",
                table: "Negocios",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_ClienteId",
                table: "NotasCredito",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_Estado",
                table: "NotasCredito",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_FacturaId",
                table: "NotasCredito",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_FechaElaboracion",
                table: "NotasCredito",
                column: "FechaElaboracion");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_NumeroNota",
                table: "NotasCredito",
                column: "NumeroNota");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_Tipo",
                table: "NotasCredito",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCredito_UsuarioId",
                table: "NotasCredito",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_ClienteId",
                table: "NotasDebito",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_ClienteId1",
                table: "NotasDebito",
                column: "ClienteId1");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_Estado",
                table: "NotasDebito",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_FacturaId",
                table: "NotasDebito",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_FacturaId1",
                table: "NotasDebito",
                column: "FacturaId1");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_FechaElaboracion",
                table: "NotasDebito",
                column: "FechaElaboracion");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_NumeroNota",
                table: "NotasDebito",
                column: "NumeroNota");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_UsuarioId",
                table: "NotasDebito",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasDebito_UsuarioId1",
                table: "NotasDebito",
                column: "UsuarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFeature_PlanFacturacionId",
                table: "PlanFeature",
                column: "PlanFacturacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_UsuarioId",
                table: "Productos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UsuarioId",
                table: "RefreshTokens",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_SuscripcionesFacturacion_PlanFacturacionId",
                table: "SuscripcionesFacturacion",
                column: "PlanFacturacionId");

            migrationBuilder.CreateIndex(
                name: "IX_SuscripcionesFacturacion_UsuarioId",
                table: "SuscripcionesFacturacion",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionesDian");

            migrationBuilder.DropTable(
                name: "DetalleFacturas");

            migrationBuilder.DropTable(
                name: "DetalleNotaCredito");

            migrationBuilder.DropTable(
                name: "DetalleNotaDebito");

            migrationBuilder.DropTable(
                name: "DocumentosSoporte");

            migrationBuilder.DropTable(
                name: "FormasPagoNotaCredito");

            migrationBuilder.DropTable(
                name: "FormasPagoNotaDebito");

            migrationBuilder.DropTable(
                name: "PlanFeature");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "SuscripcionesFacturacion");

            migrationBuilder.DropTable(
                name: "Negocios");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "NotasCredito");

            migrationBuilder.DropTable(
                name: "NotasDebito");

            migrationBuilder.DropTable(
                name: "PlanesFacturacion");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
