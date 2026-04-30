using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FactCloudAPI.Migrations
{
    /// <inheritdoc />
    public partial class Fix_TipoImpuesto_MaxLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Unidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanesFacturacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrecioAnual = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Destacado = table.Column<bool>(type: "bit", nullable: false),
                    DescuentoPorcentaje = table.Column<int>(type: "int", precision: 5, scale: 2, nullable: true),
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
                name: "RegistrosPendientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransaccionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DatosRegistro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatosNegocio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatosPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "PENDING"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotasError = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosPendientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WompiId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    DatosRegistro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatosNegocio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.Id);
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
                    FotoPerfilId = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaDesactivacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cupones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DescuentoPorcentaje = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
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
                    CodigoSucursal = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartamentoCodigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CiudadCodigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ActividadEconomicaCIIU = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegimenTributario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RegimenFiscal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NombreContactoFacturacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApellidoContactoFacturacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IndicativoFacturacion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TelefonoFacturacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GranContribuyente = table.Column<bool>(type: "bit", nullable: false),
                    AutoretenedorRenta = table.Column<bool>(type: "bit", nullable: false),
                    RetenedorIVA = table.Column<bool>(type: "bit", nullable: false),
                    RegimenSimple = table.Column<bool>(type: "bit", nullable: false),
                    NoAplica = table.Column<bool>(type: "bit", nullable: false),
                    RetenedorICA = table.Column<bool>(type: "bit", nullable: false),
                    RetenedorRenta = table.Column<bool>(type: "bit", nullable: false),
                    EsProveedor = table.Column<bool>(type: "bit", nullable: false),
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
                name: "CuentasContables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    CodigoPadre = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    ClasePUC = table.Column<int>(type: "int", nullable: false),
                    Naturaleza = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: "D"),
                    TipoAjuste = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false, defaultValue: "N"),
                    PermiteMovimiento = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RequiereTercero = table.Column<bool>(type: "bit", nullable: false),
                    RequiereCentroCosto = table.Column<bool>(type: "bit", nullable: false),
                    RequiereDocumento = table.Column<bool>(type: "bit", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentasContables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuentasContables_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                name: "FotoPerfils",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UrlExterna = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaSubida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EsPrincipal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotoPerfils", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FotoPerfils_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialSesiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Navegador = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SistemaOperativo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Dispositivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Exitoso = table.Column<bool>(type: "bit", nullable: false),
                    SesionActual = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialSesiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialSesiones_Usuarios_UsuarioId",
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
                    TipoSujeto = table.Column<int>(type: "int", nullable: false),
                    TipoDocumento = table.Column<int>(type: "int", nullable: false),
                    NombreComercial = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RazonSocial = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PrimerNombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SegundoNombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PrimerApellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SegundoApellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumeroIdentificacionE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DvNit = table.Column<int>(type: "int", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false, defaultValue: "CO"),
                    Telefono = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorreoRecepcionDian = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatosFacturacionCompletos = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
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
                    EsServicio = table.Column<bool>(type: "bit", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CodigoInterno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CodigoUNSPSC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnidadMedida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Unidad"),
                    Marca = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CodigoBarras = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncluyeIVA = table.Column<bool>(type: "bit", nullable: false),
                    ImpuestoCargo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Retencion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CantidadDisponible = table.Column<int>(type: "int", nullable: true),
                    CantidadMinima = table.Column<int>(type: "int", nullable: false),
                    TipoProducto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    TransaccionId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuscripcionesFacturacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuscripcionesFacturacion_PlanesFacturacion_PlanFacturacionId",
                        column: x => x.PlanFacturacionId,
                        principalTable: "PlanesFacturacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuscripcionesFacturacion_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "ContactosCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Indicativo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactosCliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactosCliente_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
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
                    NumeroAutorizacion = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    FechaInicioAutorizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFinAutorizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RangoNumeracionDesde = table.Column<long>(type: "bigint", nullable: false),
                    RangoNumeracionHasta = table.Column<long>(type: "bigint", nullable: false),
                    ClaveTecnica = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TipoAmbiente = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    TipoFactura = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false, defaultValue: "01"),
                    TipoOperacion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "10"),
                    NumeroFactura = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prefijo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraEmision = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    Cufe = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    QRCode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalIVA = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalINC = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalICA = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalDescuentos = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalRetenciones = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalFactura = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FormaPago = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "1"),
                    MedioPago = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "10"),
                    DiasCredito = table.Column<int>(type: "int", nullable: true),
                    MontoPagado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Emitida"),
                    Observaciones = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnviadaDIAN = table.Column<bool>(type: "bit", nullable: false),
                    FechaLimiteEnvioDIAN = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEnvioDIAN = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RespuestaDIAN = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EnviadaCliente = table.Column<bool>(type: "bit", nullable: false),
                    FechaEnvioCliente = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                name: "TelefonoCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Indicativo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Numero = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelefonoCliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelefonoCliente_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Autorretenciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoAutoretencion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Tarifa = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    CuentaDebitoId = table.Column<int>(type: "int", nullable: true),
                    CuentaCreditoId = table.Column<int>(type: "int", nullable: true),
                    BaseMinimaAplicacion = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TipoBase = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "Pesos"),
                    EnUso = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autorretenciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Autorretenciones_CuentasContables_CuentaCreditoId",
                        column: x => x.CuentaCreditoId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Autorretenciones_CuentasContables_CuentaDebitoId",
                        column: x => x.CuentaDebitoId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Autorretenciones_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Impuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoImpuesto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Tarifa = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    PorValor = table.Column<bool>(type: "bit", nullable: false),
                    CodigoTributoDIAN = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    CuentaDebitoVentasId = table.Column<int>(type: "int", nullable: true),
                    CuentaCreditoVentasId = table.Column<int>(type: "int", nullable: true),
                    CuentaDebitoComprasId = table.Column<int>(type: "int", nullable: true),
                    CuentaCreditoComprasId = table.Column<int>(type: "int", nullable: true),
                    CuentaDevolucionVentasId = table.Column<int>(type: "int", nullable: true),
                    CuentaDevolucionComprasId = table.Column<int>(type: "int", nullable: true),
                    EnUso = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impuestos_CuentasContables_CuentaCreditoComprasId",
                        column: x => x.CuentaCreditoComprasId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Impuestos_CuentasContables_CuentaCreditoVentasId",
                        column: x => x.CuentaCreditoVentasId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Impuestos_CuentasContables_CuentaDebitoComprasId",
                        column: x => x.CuentaDebitoComprasId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Impuestos_CuentasContables_CuentaDebitoVentasId",
                        column: x => x.CuentaDebitoVentasId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Impuestos_CuentasContables_CuentaDevolucionComprasId",
                        column: x => x.CuentaDevolucionComprasId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Impuestos_CuentasContables_CuentaDevolucionVentasId",
                        column: x => x.CuentaDevolucionVentasId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Impuestos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionesDian",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoftwareProveedor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SoftwarePIN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrefijoAutorizadoDIAN = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    NumeroResolucionDIAN = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    RangoNumeracionDesde = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RangoNumeracionHasta = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AmbienteDIAN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Habilitacion"),
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
                name: "PerfilesTributarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NegocioId = table.Column<int>(type: "int", nullable: false),
                    RegimenIvaCodigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ActividadEconomicaCIIU = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TributosJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsabilidadesFiscalesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilesTributarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilesTributarios_Negocios_NegocioId",
                        column: x => x.NegocioId,
                        principalTable: "Negocios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepresentantesLegales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NegocioId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoDocumento = table.Column<int>(type: "int", nullable: false),
                    NumeroIdentificacion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CiudadExpedicion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CiudadResidencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepresentantesLegales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepresentantesLegales_Negocios_NegocioId",
                        column: x => x.NegocioId,
                        principalTable: "Negocios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResolucionesDIAN",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NegocioId = table.Column<int>(type: "int", nullable: false),
                    NumeroAutorizacion = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Prefijo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RangoDesde = table.Column<long>(type: "bigint", nullable: false),
                    RangoHasta = table.Column<long>(type: "bigint", nullable: false),
                    ClaveTecnica = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TipoAmbiente = table.Column<int>(type: "int", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolucionesDIAN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResolucionesDIAN_Negocios_NegocioId",
                        column: x => x.NegocioId,
                        principalTable: "Negocios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    DescuentoAplicado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "DetalleFacturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Unidad"),
                    Cantidad = table.Column<decimal>(type: "decimal(12,6)", precision: 12, scale: 6, nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false, defaultValue: 0m),
                    ValorDescuento = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SubtotalLinea = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TarifaIVA = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false, defaultValue: 0m),
                    ValorIVA = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TarifaINC = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false, defaultValue: 0m),
                    ValorINC = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TarifaICA = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false, defaultValue: 0m),
                    ValorICA = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalLinea = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CodigoUNSPSC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CodigoInterno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                name: "DetalleFacturaImpuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetalleFacturaId = table.Column<int>(type: "int", nullable: false),
                    ImpuestoId = table.Column<int>(type: "int", nullable: false),
                    BaseGravable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaAplicada = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    ValorImpuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NaturalezaImpuesto = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false, defaultValue: "Cargo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleFacturaImpuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleFacturaImpuestos_DetalleFacturas_DetalleFacturaId",
                        column: x => x.DetalleFacturaId,
                        principalTable: "DetalleFacturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleFacturaImpuestos_Impuestos_ImpuestoId",
                        column: x => x.ImpuestoId,
                        principalTable: "Impuestos",
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
                table: "Addons",
                columns: new[] { "Id", "Activo", "Color", "Descripcion", "Nombre", "Precio", "Tipo", "Unidad" },
                values: new object[,]
                {
                    { 1, true, "#1a73e8", "Agrega 150 documentos electrónicos adicionales a tu plan actual.", "Documentos extra (150)", 45000m, "Capacidad", "año" },
                    { 2, true, "#1a73e8", "Agrega 500 documentos electrónicos adicionales a tu plan actual.", "Documentos extra (500)", 120000m, "Capacidad", "año" },
                    { 3, true, "#34a853", "Permite añadir un usuario adicional a tu cuenta.", "Usuario adicional", 80000m, "Usuarios", "año" }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 1001, true, 1, "13551501", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 2.5%", true, false, true, true, "M", null },
                    { 1002, true, 1, "13551503", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 4%", true, false, true, true, "M", null },
                    { 1003, true, 1, "13551505", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 6%", true, false, true, true, "M", null },
                    { 1004, true, 1, "13551507", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 10%", true, false, true, true, "M", null },
                    { 1005, true, 1, "13551509", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 11%", true, false, true, true, "M", null },
                    { 1006, true, 1, "13551519", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Autorretención especial en renta", true, false, false, false, "M", null },
                    { 1007, true, 1, "13551801", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete ICA 11.04", true, false, true, true, "M", null },
                    { 1008, true, 1, "13551803", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete ICA 13.08", true, false, true, true, "M", null },
                    { 1009, true, 1, "13551805", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete ICA 9.66", true, false, true, true, "M", null },
                    { 1010, true, 1, "13551511", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 7%", true, false, true, true, "M", null },
                    { 1011, true, 1, "13551513", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 3.5%", true, false, true, true, "M", null },
                    { 1012, true, 1, "13551515", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 2%", true, false, true, true, "M", null },
                    { 1013, true, 1, "13551517", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 1%", true, false, true, true, "M", null },
                    { 1014, true, 1, "13551701", "135517", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto a las ventas retenido 15%", true, false, true, false, "M", null },
                    { 1015, true, 1, "13551703", "135517", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto a las ventas retenido 100%", true, false, true, false, "M", null },
                    { 1016, true, 1, "13551807", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete ICA 8", true, false, true, true, "M", null },
                    { 1017, true, 1, "13551809", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete ICA 7", true, false, true, true, "M", null },
                    { 1018, true, 1, "13551811", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete ICA 6.9", true, false, true, true, "M", null },
                    { 1019, true, 1, "13551813", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete ICA 4.14", true, false, true, true, "M", null },
                    { 2001, true, 2, "24080601", "240806", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "IVA generado 19%", true, false, true, false, "M", null },
                    { 2002, true, 2, "24080602", "240806", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "IVA generado 5%", true, false, true, false, "M", null },
                    { 2003, true, 2, "24081001", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "IVA descontable por compras 19%", true, false, true, false, "M", null },
                    { 2004, true, 2, "24081003", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "IVA descontable por compras 5%", true, false, true, false, "M", null },
                    { 2005, true, 2, "24082001", "240820", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Descontable por devoluciones 19%", true, false, true, false, "M", null },
                    { 2006, true, 2, "24082002", "240820", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Descontable por devoluciones 5%", true, false, true, false, "M", null },
                    { 2007, true, 2, "24081002", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "IVA Devolución en compras 19%", true, false, true, false, "M", null },
                    { 2008, true, 2, "24081004", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "IVA Devolución en compras 5%", true, false, true, false, "M", null },
                    { 2009, true, 2, "23651501", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Honorarios — retención a cargo", true, false, true, true, "M", null },
                    { 2010, true, 2, "23651502", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución retención en la fuente 11%", true, false, true, true, "M", null },
                    { 2011, true, 2, "23652001", "236520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Comisiones — retención a cargo", true, false, true, true, "M", null },
                    { 2012, true, 2, "23654001", "236540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Retención por compras 2.5%", true, false, true, true, "M", null },
                    { 2013, true, 2, "23652501", "236525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Servicios 6% — retención a cargo", true, false, true, true, "M", null },
                    { 2014, true, 2, "23652503", "236525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Servicios 4% — retención a cargo", true, false, true, true, "M", null },
                    { 2015, true, 2, "23657501", "236575", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Autorretenciones por pagar", true, false, false, false, "M", null },
                    { 2016, true, 2, "23680501", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "ReteICA 11.04 por pagar", true, false, true, true, "M", null },
                    { 2017, true, 2, "23680503", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "ReteICA 13.08 por pagar", true, false, true, true, "M", null },
                    { 2018, true, 2, "23680505", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "ReteICA 9.66 por pagar", true, false, true, true, "M", null },
                    { 2019, true, 2, "23680507", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "ReteICA 8 por pagar", true, false, true, true, "M", null },
                    { 2020, true, 2, "23680509", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "ReteICA 7 por pagar", true, false, true, true, "M", null },
                    { 2021, true, 2, "23680511", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "ReteICA 6.9 por pagar", true, false, true, true, "M", null },
                    { 2022, true, 2, "23680513", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "ReteICA 4.14 por pagar", true, false, true, true, "M", null },
                    { 2023, true, 2, "23670101", "236701", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "ReteIVA 15% por pagar", true, false, true, false, "M", null },
                    { 2024, true, 2, "23670103", "236701", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "ReteIVA 100% por pagar", true, false, true, false, "M", null },
                    { 2025, true, 2, "24950101", "249501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto al consumo en ventas", true, false, true, false, "M", null },
                    { 2026, true, 2, "24950102", "249501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución impoconsumo en ventas", true, false, true, false, "M", null },
                    { 2027, true, 2, "24950103", "249501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto al consumo en compras", true, false, true, false, "M", null },
                    { 2028, true, 2, "24950104", "249501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución impoconsumo en compras", true, false, true, false, "M", null },
                    { 2029, true, 2, "246401", "246400", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto por valor en ventas", true, false, true, false, "M", null },
                    { 2030, true, 2, "246402", "246400", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución impuesto por valor en ventas", true, false, true, false, "M", null },
                    { 2031, true, 2, "246403", "246400", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto por valor en compras", true, false, true, false, "M", null },
                    { 2032, true, 2, "246404", "246400", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución impuesto por valor en compras", true, false, true, false, "M", null },
                    { 2033, true, 2, "23653502", "236535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Rendimientos financieros 7% — retención a cargo", true, false, true, true, "M", null },
                    { 2034, true, 2, "23654003", "236540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Retención por compras 3.5%", true, false, true, true, "M", null },
                    { 2035, true, 2, "23657001", "236570", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Otras retenciones 2%", true, false, true, true, "M", null },
                    { 2036, true, 2, "23652505", "236525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Servicios 1% — retención a cargo", true, false, true, true, "M", null },
                    { 2037, true, 2, "24080603", "240806", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "IVA generado 16%", true, false, true, false, "M", null },
                    { 2038, true, 2, "24081005", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "IVA descontable por compras 16%", true, false, true, false, "M", null },
                    { 2039, true, 2, "24082003", "240820", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Descontable por devoluciones 16%", true, false, true, false, "M", null },
                    { 2040, true, 2, "24081006", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "IVA Devolución en compras 16%", true, false, true, false, "M", null },
                    { 2041, true, 2, "24640501", "246405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto Ad valorem en ventas 20%", true, false, true, false, "M", null },
                    { 2042, true, 2, "24640601", "246406", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Ad valorem en ventas 20%", true, false, true, false, "M", null },
                    { 2043, true, 2, "24640502", "246405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto Ad valorem en ventas 25%", true, false, true, false, "M", null },
                    { 2044, true, 2, "24640602", "246406", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Ad valorem en ventas 25%", true, false, true, false, "M", null },
                    { 2045, true, 2, "24651005", "246510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Ventas — Comestibles ultraprocesados 15%", true, false, true, false, "M", null },
                    { 2046, true, 2, "24651006", "246510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución ventas — Comestibles ultraprocesados 15%", true, false, true, false, "M", null },
                    { 2047, true, 2, "24651007", "246510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Compras — Comestibles ultraprocesados 15%", true, false, true, false, "M", null },
                    { 2048, true, 2, "24651008", "246510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución compras — Comestibles ultraprocesados 15%", true, false, true, false, "M", null },
                    { 2049, true, 2, "24651009", "246510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Ventas — Comestibles ultraprocesados 20%", true, false, true, false, "M", null },
                    { 2050, true, 2, "24651010", "246510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución ventas — Comestibles ultraprocesados 20%", true, false, true, false, "M", null },
                    { 2051, true, 2, "24651011", "246510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Compras — Comestibles ultraprocesados 20%", true, false, true, false, "M", null },
                    { 2052, true, 2, "24651012", "246510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución compras — Comestibles ultraprocesados 20%", true, false, true, false, "M", null },
                    { 5001, true, 5, "51159502", "511595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto Ad valorem en compras 20%", true, false, true, false, "M", null },
                    { 5002, true, 5, "51159504", "511595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Ad valorem en compras 20%", true, false, true, false, "M", null },
                    { 5003, true, 5, "51159503", "511595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto Ad valorem en compras 25%", true, false, true, false, "M", null },
                    { 5004, true, 5, "51159505", "511595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Ad valorem en compras 25%", true, false, true, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "Cupones",
                columns: new[] { "Id", "Codigo", "DescuentoPorcentaje", "Expiracion", "IsActive", "MaxUsos", "PlanId", "UsosCodigo" },
                values: new object[] { 1, "WELCOMEFC", 20m, null, true, 30, null, 0 });

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
                table: "Autorretenciones",
                columns: new[] { "Id", "BaseMinimaAplicacion", "Codigo", "CuentaCreditoId", "CuentaDebitoId", "EnUso", "FechaCreacion", "Nombre", "Tarifa", "TipoAutoretencion", "TipoBase", "UsuarioId" },
                values: new object[,]
                {
                    { 1, null, 26, 2015, 1006, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Autorretención 0.40%", 0.40m, "Autoretención 2201", "Pesos", null },
                    { 2, null, 27, 2015, 1006, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Autorretención 0.80%", 0.80m, "Autoretención 2201", "Pesos", null },
                    { 3, null, 28, 2015, 1006, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Autorretención 1.60%", 1.60m, "Autoretención 2201", "Pesos", null }
                });

            migrationBuilder.InsertData(
                table: "Cupones",
                columns: new[] { "Id", "Codigo", "DescuentoPorcentaje", "Expiracion", "IsActive", "MaxUsos", "PlanId", "UsosCodigo" },
                values: new object[,]
                {
                    { 2, "FACTCLOUDPRO", 30m, null, true, 30, 3, 0 },
                    { 3, "STARTEFC25", 12m, null, true, 20, 1, 0 }
                });

            migrationBuilder.InsertData(
                table: "Impuestos",
                columns: new[] { "Id", "Codigo", "CodigoTributoDIAN", "CuentaCreditoComprasId", "CuentaCreditoVentasId", "CuentaDebitoComprasId", "CuentaDebitoVentasId", "CuentaDevolucionComprasId", "CuentaDevolucionVentasId", "EnUso", "FechaCreacion", "Nombre", "PorValor", "Tarifa", "TipoImpuesto", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 1, "01", null, 2001, 2003, null, 2007, 2005, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IVA 19%", false, 19.00m, "IVA", null },
                    { 2, 2, "01", null, 2002, 2004, null, 2008, 2006, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IVA 5%", false, 5.00m, "IVA", null },
                    { 3, 3, "05", 2009, null, null, 1005, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 11%", false, 11.00m, "Retefuente", null },
                    { 4, 4, "05", 2011, null, null, 1004, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 10%", false, 10.00m, "Retefuente", null },
                    { 5, 5, "05", 2013, null, null, 1003, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 6%", false, 6.00m, "Retefuente", null },
                    { 6, 6, "05", 2014, null, null, 1002, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 4%", false, 4.00m, "Retefuente", null },
                    { 7, 7, "05", 2012, null, null, 1001, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 2.5%", false, 2.50m, "Retefuente", null },
                    { 8, 8, "06", 2016, null, null, 1007, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 11.04", false, 11.04m, "ReteICA", null },
                    { 9, 9, "06", 2017, null, null, 1008, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 13.8", false, 13.80m, "ReteICA", null },
                    { 10, 10, "06", 2018, null, null, 1009, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 9.66", false, 9.66m, "ReteICA", null },
                    { 11, 11, "06", 2019, null, null, 1016, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 8", false, 8.00m, "ReteICA", null },
                    { 12, 12, "06", 2020, null, null, 1017, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 7", false, 7.00m, "ReteICA", null },
                    { 13, 13, "06", 2021, null, null, 1018, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 6.9", false, 6.90m, "ReteICA", null },
                    { 14, 14, "06", 2022, null, null, 1019, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 4.14", false, 4.14m, "ReteICA", null },
                    { 15, 15, "04", 2023, null, null, 1014, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteIVA 15%", false, 15.00m, "ReteIVA", null },
                    { 16, 16, "02", null, 2025, 2027, null, 2028, 2026, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Impoconsumo 8%", false, 8.00m, "Impoconsumo", null },
                    { 17, 17, "02", null, 2029, 2031, null, 2032, 2030, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Impoconsumo por valor", false, 0.00m, "Impoconsumo", null },
                    { 18, 18, "05", 2034, null, null, 1011, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 3.5%", false, 3.50m, "Retefuente", null },
                    { 19, 19, "05", 2033, null, null, 1010, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 7%", false, 7.00m, "Retefuente", null },
                    { 20, 20, "05", 2035, null, null, 1012, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 2%", false, 2.00m, "Retefuente", null },
                    { 21, 21, "05", 2036, null, null, 1013, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 1%", false, 1.00m, "Retefuente", null },
                    { 22, 22, "01", null, 2001, 2003, null, 2007, 2005, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IVA 0%", false, 0.00m, "IVA", null },
                    { 23, 23, "01", null, 2037, 2038, null, 2040, 2039, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IVA 16%", false, 16.00m, "IVA", null },
                    { 24, 24, "ZY", null, 2041, 5001, null, 5002, 2042, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AdValorem 20%", false, 20.00m, "Ad-Valorem", null },
                    { 25, 25, "ZY", null, 2043, 5003, null, 5004, 2044, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AdValorem 25%", false, 25.00m, "Ad-Valorem", null },
                    { 29, 29, "04", 2024, null, null, 1015, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteIVA 100%", false, 100.00m, "ReteIVA", null },
                    { 90, 90, "05", 2012, null, null, 1001, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 1.5%", false, 1.50m, "Retefuente", null },
                    { 91, 91, "05", 2012, null, null, 1001, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 0.10%", false, 0.10m, "Retefuente", null },
                    { 92, 92, "05", 2012, null, null, 1001, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 0.50%", false, 0.50m, "Retefuente", null },
                    { 93, 93, "05", 2012, null, null, 1001, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 20%", false, 20.00m, "Retefuente", null },
                    { 94, 94, "ZA", null, 2045, 2047, null, 2048, 2046, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comestibles ultraprocesados 15%", false, 15.00m, "Comestibles ultraprocesados", null },
                    { 95, 95, "ZA", null, 2049, 2051, null, 2052, 2050, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comestibles ultraprocesados 20%", false, 20.00m, "Comestibles ultraprocesados", null }
                });

            migrationBuilder.InsertData(
                table: "PlanFeature",
                columns: new[] { "Id", "PlanFacturacionId", "Texto", "Tooltip" },
                values: new object[,]
                {
                    { 1, 1, "1 Usuario", "Cuenta individual para emprendedores que están empezando." },
                    { 2, 1, "30 Documentos anuales", "Emite hasta 30 facturas electrónicas al año." },
                    { 3, 1, "Funciones básicas", "Creación de facturas, gestión de clientes y productos. Reportes simples incluidos." },
                    { 4, 2, "1 Usuario", "Cuenta individual perfecta para emprendedores y negocios unipersonales." },
                    { 5, 2, "140 Documentos electrónicos al año", "Perfecto para negocios que emiten hasta 8 documentos diarios." },
                    { 6, 2, "Funciones básicas", "Creación de facturas, gestión de clientes, productos, notas débito y crédito." },
                    { 7, 3, "1 Usuario", "Cuenta individual con acceso completo a todas las funcionalidades." },
                    { 8, 3, "540 Documentos electrónicos al año", "Ideal para PYMES que facturan de forma constante durante todo el año." },
                    { 9, 3, "Facturación electrónica DIAN", "Emisión de facturas electrónicas válidas ante la DIAN." },
                    { 10, 3, "Notas crédito y débito", "Corrección y ajustes de facturas mediante notas crédito y débito electrónicas." },
                    { 11, 3, "Gestión avanzada de clientes y productos", "Administra clientes, productos, precios e impuestos de forma organizada." },
                    { 12, 3, "Reportes y control de facturación", "Consulta reportes básicos de ventas, documentos emitidos y estado de facturación." },
                    { 13, 4, "1 Usuario", "Acceso completo al sistema con control total de la facturación empresarial." },
                    { 14, 4, "1550 Documentos electrónicos al año", "Pensado para empresas con alto volumen de facturación anual." },
                    { 15, 4, "Facturación electrónica DIAN", "Cumple con todos los requisitos exigidos por la DIAN." },
                    { 16, 4, "Notas crédito y débito ilimitadas", "Emite notas crédito y débito sin restricciones dentro del límite anual." },
                    { 17, 4, "Gestión completa de clientes y productos", "Control detallado de clientes, productos, impuestos y precios." },
                    { 18, 4, "Reportes administrativos", "Accede a reportes de ventas y facturación para control interno y contable." },
                    { 19, 4, "Soporte prioritario", "Atención prioritaria para resolución de dudas y soporte técnico." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Autoretencion_Usuario_Codigo",
                table: "Autorretenciones",
                columns: new[] { "UsuarioId", "Codigo" },
                unique: true,
                filter: "[UsuarioId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Autorretenciones_CuentaCreditoId",
                table: "Autorretenciones",
                column: "CuentaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_Autorretenciones_CuentaDebitoId",
                table: "Autorretenciones",
                column: "CuentaDebitoId");

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
                name: "IX_ContactosCliente_ClienteId",
                table: "ContactosCliente",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentaContable_Usuario_Codigo",
                table: "CuentasContables",
                columns: new[] { "UsuarioId", "Codigo" },
                unique: true,
                filter: "[UsuarioId] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturaImpuesto_Detalle_Impuesto",
                table: "DetalleFacturaImpuestos",
                columns: new[] { "DetalleFacturaId", "ImpuestoId" });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturaImpuestos_ImpuestoId",
                table: "DetalleFacturaImpuestos",
                column: "ImpuestoId");

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
                name: "IX_Facturas_Cufe",
                table: "Facturas",
                column: "Cufe");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_EnviadaDIAN",
                table: "Facturas",
                column: "EnviadaDIAN");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_Estado",
                table: "Facturas",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_FechaLimiteEnvioDIAN",
                table: "Facturas",
                column: "FechaLimiteEnvioDIAN");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_FechaVencimiento",
                table: "Facturas",
                column: "FechaVencimiento");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_Prefijo_NumeroFactura",
                table: "Facturas",
                columns: new[] { "Prefijo", "NumeroFactura" });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UsuarioId",
                table: "Facturas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UsuarioId_FechaEmision",
                table: "Facturas",
                columns: new[] { "UsuarioId", "FechaEmision" });

            migrationBuilder.CreateIndex(
                name: "IX_FormasPagoNotaCredito_NotaCreditoId",
                table: "FormasPagoNotaCredito",
                column: "NotaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_FormasPagoNotaDebito_NotaDebitoId",
                table: "FormasPagoNotaDebito",
                column: "NotaDebitoId");

            migrationBuilder.CreateIndex(
                name: "IX_FotoPerfils_UsuarioId",
                table: "FotoPerfils",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialSesiones_UsuarioId",
                table: "HistorialSesiones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Impuesto_Usuario_Codigo",
                table: "Impuestos",
                columns: new[] { "UsuarioId", "Codigo" },
                unique: true,
                filter: "[UsuarioId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_CuentaCreditoComprasId",
                table: "Impuestos",
                column: "CuentaCreditoComprasId");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_CuentaCreditoVentasId",
                table: "Impuestos",
                column: "CuentaCreditoVentasId");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_CuentaDebitoComprasId",
                table: "Impuestos",
                column: "CuentaDebitoComprasId");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_CuentaDebitoVentasId",
                table: "Impuestos",
                column: "CuentaDebitoVentasId");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_CuentaDevolucionComprasId",
                table: "Impuestos",
                column: "CuentaDevolucionComprasId");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_CuentaDevolucionVentasId",
                table: "Impuestos",
                column: "CuentaDevolucionVentasId");

            migrationBuilder.CreateIndex(
                name: "IX_Negocios_NumeroIdentificacionE",
                table: "Negocios",
                column: "NumeroIdentificacionE",
                unique: true);

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
                name: "IX_PerfilesTributarios_NegocioId",
                table: "PerfilesTributarios",
                column: "NegocioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanFeature_PlanFacturacionId",
                table: "PlanFeature",
                column: "PlanFacturacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CodigoInterno",
                table: "Productos",
                column: "CodigoInterno");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CodigoUNSPSC",
                table: "Productos",
                column: "CodigoUNSPSC");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_UsuarioId",
                table: "Productos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_UsuarioId_Activo",
                table: "Productos",
                columns: new[] { "UsuarioId", "Activo" });

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
                name: "IX_RegistrosPendientes_Email",
                table: "RegistrosPendientes",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPendientes_Estado",
                table: "RegistrosPendientes",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPendientes_TransaccionId",
                table: "RegistrosPendientes",
                column: "TransaccionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepresentantesLegales_NegocioId",
                table: "RepresentantesLegales",
                column: "NegocioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResolucionesDIAN_NegocioId",
                table: "ResolucionesDIAN",
                column: "NegocioId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolucionesDIAN_NegocioId_Activa",
                table: "ResolucionesDIAN",
                columns: new[] { "NegocioId", "Activa" });

            migrationBuilder.CreateIndex(
                name: "IX_SuscripcionesFacturacion_PlanFacturacionId",
                table: "SuscripcionesFacturacion",
                column: "PlanFacturacionId");

            migrationBuilder.CreateIndex(
                name: "IX_SuscripcionesFacturacion_UsuarioId",
                table: "SuscripcionesFacturacion",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TelefonoCliente_ClienteId",
                table: "TelefonoCliente",
                column: "ClienteId");

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
                name: "Autorretenciones");

            migrationBuilder.DropTable(
                name: "ConfiguracionesDian");

            migrationBuilder.DropTable(
                name: "ContactosCliente");

            migrationBuilder.DropTable(
                name: "CuponUso");

            migrationBuilder.DropTable(
                name: "DetalleFacturaImpuestos");

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
                name: "FotoPerfils");

            migrationBuilder.DropTable(
                name: "HistorialSesiones");

            migrationBuilder.DropTable(
                name: "PerfilesTributarios");

            migrationBuilder.DropTable(
                name: "PlanFeature");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RegistrosPendientes");

            migrationBuilder.DropTable(
                name: "RepresentantesLegales");

            migrationBuilder.DropTable(
                name: "ResolucionesDIAN");

            migrationBuilder.DropTable(
                name: "TelefonoCliente");

            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "UsuariosAddons");

            migrationBuilder.DropTable(
                name: "Cupones");

            migrationBuilder.DropTable(
                name: "SuscripcionesFacturacion");

            migrationBuilder.DropTable(
                name: "DetalleFacturas");

            migrationBuilder.DropTable(
                name: "Impuestos");

            migrationBuilder.DropTable(
                name: "NotasCredito");

            migrationBuilder.DropTable(
                name: "NotasDebito");

            migrationBuilder.DropTable(
                name: "Negocios");

            migrationBuilder.DropTable(
                name: "Addons");

            migrationBuilder.DropTable(
                name: "PlanesFacturacion");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "CuentasContables");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
