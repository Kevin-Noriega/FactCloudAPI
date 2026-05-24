using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NubeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                name: "ImpuestosConceptos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: true),
                    CodigoInterno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    CodigoTributoDIAN = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    EsRetencion = table.Column<bool>(type: "bit", nullable: false),
                    EsAutorretencion = table.Column<bool>(type: "bit", nullable: false),
                    RequiereBaseMinima = table.Column<bool>(type: "bit", nullable: false),
                    PermiteTarifaCero = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpuestosConceptos", x => x.Id);
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
                    WompiReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "PENDING"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TipoIdentificacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumeroIdentificacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NombreNegocio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Nit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DvNit = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TelefonoNegocio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CorreoNegocio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PlanFacturacionId = table.Column<int>(type: "int", nullable: false),
                    FechaAprobacion = table.Column<DateTime>(type: "datetime2", maxLength: 20, nullable: true),
                    DatosRegistro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatosNegocio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatosPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                name: "TarifasImpuestos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImpuestoConceptoId = table.Column<long>(type: "bigint", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoMonto = table.Column<int>(type: "int", nullable: false),
                    Tarifa = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BaseMinima = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    UnidadBaseMinima = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PrecioIncluyeImpuesto = table.Column<bool>(type: "bit", nullable: false),
                    PermiteAcumulacionConOtros = table.Column<bool>(type: "bit", nullable: false),
                    PrioridadCalculo = table.Column<short>(type: "smallint", nullable: false),
                    VigenteDesde = table.Column<DateOnly>(type: "date", nullable: false),
                    VigenteHasta = table.Column<DateOnly>(type: "date", nullable: true),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarifasImpuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TarifasImpuestos_ImpuestosConceptos_ImpuestoConceptoId",
                        column: x => x.ImpuestoConceptoId,
                        principalTable: "ImpuestosConceptos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    MunicipioId = table.Column<int>(type: "int", nullable: true),
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
                    NombreNegocio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nit = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DvNit = table.Column<int>(type: "int", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Departamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "CO"),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatosFacturacionCompletos = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
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
                name: "ConfiguracionesImpuestoEmpresa",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    TarifaImpuestoId = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    AplicacionAutomatica = table.Column<bool>(type: "bit", nullable: false),
                    PermiteEdicionManual = table.Column<bool>(type: "bit", nullable: false),
                    GeneraContabilidad = table.Column<bool>(type: "bit", nullable: false),
                    ReportaDIAN = table.Column<bool>(type: "bit", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionesImpuestoEmpresa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracionesImpuestoEmpresa_TarifasImpuestos_TarifaImpuestoId",
                        column: x => x.TarifaImpuestoId,
                        principalTable: "TarifasImpuestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentosResumenImpuesto",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    TarifaImpuestoId = table.Column<long>(type: "bigint", nullable: false),
                    Naturaleza = table.Column<int>(type: "int", nullable: false),
                    BaseTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TasaAplicada = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosResumenImpuesto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentosResumenImpuesto_TarifasImpuestos_TarifaImpuestoId",
                        column: x => x.TarifaImpuestoId,
                        principalTable: "TarifasImpuestos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReglasImpuesto",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TarifaImpuestoId = table.Column<long>(type: "bigint", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CondicionJSON = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TarifaAlternativaId = table.Column<long>(type: "bigint", nullable: true),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    Prioridad = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReglasImpuesto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReglasImpuesto_TarifasImpuestos_TarifaImpuestoId",
                        column: x => x.TarifaImpuestoId,
                        principalTable: "TarifasImpuestos",
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
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CuentaContableId = table.Column<int>(type: "int", nullable: true),
                    CuentaContableId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impuestos_CuentasContables_CuentaContableId",
                        column: x => x.CuentaContableId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Impuestos_CuentasContables_CuentaContableId1",
                        column: x => x.CuentaContableId1,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
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
                name: "MapeosContablesTarifa",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: true),
                    TarifaImpuestoId = table.Column<long>(type: "bigint", nullable: false),
                    Contexto = table.Column<int>(type: "int", nullable: false),
                    RolCuenta = table.Column<int>(type: "int", nullable: false),
                    CuentaContableId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapeosContablesTarifa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapeosContablesTarifa_CuentasContables_CuentaContableId",
                        column: x => x.CuentaContableId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MapeosContablesTarifa_TarifasImpuestos_TarifaImpuestoId",
                        column: x => x.TarifaImpuestoId,
                        principalTable: "TarifasImpuestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    FactusRangoId = table.Column<int>(type: "int", nullable: true),
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
                    NaturalezaImpuesto = table.Column<int>(type: "int", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentosLineasImpuesto",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetalleFacturaId = table.Column<int>(type: "int", nullable: false),
                    TarifaImpuestoId = table.Column<long>(type: "bigint", nullable: false),
                    BaseGravable = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TarifaUtilizada = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ValorCalculado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Naturaleza = table.Column<int>(type: "int", nullable: false),
                    SnapshotNombreTarifa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SnapshotCodigoDIAN = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    SnapshotTarifa = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ReglaAplicada = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaCalculo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosLineasImpuesto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentosLineasImpuesto_DetalleFacturas_DetalleFacturaId",
                        column: x => x.DetalleFacturaId,
                        principalTable: "DetalleFacturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentosLineasImpuesto_TarifasImpuestos_TarifaImpuestoId",
                        column: x => x.TarifaImpuestoId,
                        principalTable: "TarifasImpuestos",
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
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10000, 1, "1", null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 1, "Activo", false, false, false, "M", null },
                    { 10001, 1, "11", "1", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Efectivo y equivalentes de efectivo", false, false, false, "M", null },
                    { 10002, 1, "1105", "11", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Caja", false, false, false, "M", null },
                    { 10003, 1, "110505", "1105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Caja general", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10004, true, 1, "11050501", "110505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Caja general", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10005, 1, "11050597", "110505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal caja general", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10006, 1, "110510", "1105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Cajas menores", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10007, true, 1, "11051001", "110510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Cajas menores", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10008, 1, "11051097", "110510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal base cartera", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10009, 1, "1110", "11", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Bancos", false, false, false, "M", null },
                    { 10010, 1, "111005", "1110", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Moneda nacional", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10011, true, 1, "11100501", "111005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Moneda nacional", true, false, false, false, "M", null },
                    { 10012, true, 1, "11100502", "111005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Pagos en línea", true, false, false, false, "M", null },
                    { 10013, true, 1, "11100503", "111005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Pagos en línea Mercado Pago", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10014, 1, "11100597", "111005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal moneda nacional", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10015, 1, "1120", "11", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Cuentas de ahorro", false, false, false, "M", null },
                    { 10016, 1, "112005", "1120", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Bancos", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10017, true, 1, "11200501", "112005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Bancos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10018, 1, "11200597", "112005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal bancos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10019, 1, "1145", "11", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Inversiones en efectivo", false, false, false, "M", null },
                    { 10020, 1, "114505", "1145", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Fiducias", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10021, true, 1, "11450501", "114505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Fiducias", true, false, false, false, "M", null },
                    { 10022, true, 1, "11450597", "114505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal fiducias", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10023, 1, "12", "1", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Inversiones en asociadas", false, false, false, "M", null },
                    { 10024, 1, "1205", "12", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Acciones", false, false, false, "M", null },
                    { 10025, 1, "120535", "1205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Comercio al por mayor y al por menor", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10026, true, 1, "12053501", "120535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Comercio al por mayor y al por menor", true, false, false, false, "M", null },
                    { 10027, true, 1, "12053502", "120535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Reajuste fiscal", true, false, false, false, "M", null },
                    { 10028, true, 1, "12053503", "120535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Método de participación", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10029, 1, "12053597", "120535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal métodos de participación", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10030, 1, "1295", "12", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Acciones o derechos en clubes deportivos", false, false, false, "M", null },
                    { 10031, 1, "129515", "1295", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Acciones o derechos en clubes deportivos", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10032, true, 1, "12951501", "129515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Acciones o derechos en clubes deportivos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10033, 1, "12951597", "129515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal acciones o derechos en clubes deportivos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10034, 1, "129595", "1295", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otras inversiones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10035, true, 1, "12959501", "129595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otras inversiones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10036, 1, "12959597", "129595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal otras inversiones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10037, 1, "13", "1", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Deudores comerciales y otras cuentas por cobrar", false, false, false, "M", null },
                    { 10038, 1, "1305", "13", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Clientes nacionales", false, false, false, "M", null },
                    { 10039, 1, "130505", "1305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Clientes nacionales", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10040, true, 1, "13050501", "130505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Clientes nacionales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10041, 1, "13050597", "130505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal Clientes nacionales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10042, 1, "130510", "1305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Clientes del exterior", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10043, true, 1, "13051001", "130510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Clientes del exterior", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10044, 1, "13051097", "130510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal Clientes del exterior", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10045, 1, "1325", "13", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Cuentas por cobrar a socios y accionistas", false, false, false, "M", null },
                    { 10046, 1, "132510", "1325", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "A accionistas", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10047, true, 1, "13251001", "132510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "A accionistas", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10048, 1, "13251097", "132510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal a accionistas", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10049, 1, "1330", "13", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Anticipos y avances", false, false, false, "M", null },
                    { 10050, 1, "133005", "1330", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "A proveedores", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10051, true, 1, "13300501", "133005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "A proveedores", true, false, false, false, "M", null },
                    { 10052, true, 1, "13300597", "133005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal a proveedores", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10053, 1, "133010", "1330", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "A contratistas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10054, true, 1, "13301001", "133010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "A contratistas", true, false, false, false, "M", null },
                    { 10055, true, 1, "13301097", "133010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal a contratistas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10056, 1, "133015", "1330", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "A trabajadores", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10057, true, 1, "13301501", "133015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Eventos", true, false, false, false, "M", null },
                    { 10058, true, 1, "13301502", "133015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otros", true, false, false, false, "M", null },
                    { 10059, true, 1, "13301597", "133015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal anticipos a trabajadores", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10060, 1, "133095", "1330", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10061, true, 1, "13309501", "133095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10062, 1, "13309597", "133095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal Otros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10063, 1, "1355", "13", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Anticipo de impuestos y contribuciones o", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10064, true, 1, "135501", "1355", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "xxxxx", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10065, 1, "135510", "1355", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Anticipo de impuestos de industria y com.", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10066, true, 1, "13551001", "135510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo de impuestos de industria y com.", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10067, 1, "13551097", "135510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal Anticipo de impuesto industria y comercio", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10068, 1, "135515", "1355", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Anticipo Retención en la fuente", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10069, true, 1, "13551501", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 2,5%", true, false, false, false, "M", null },
                    { 10070, true, 1, "13551502", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Retención en la fuente 2,5%", true, false, false, false, "M", null },
                    { 10071, true, 1, "13551503", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 4%", true, false, false, false, "M", null },
                    { 10072, true, 1, "13551504", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Retención en la fuente 4%", true, false, false, false, "M", null },
                    { 10073, true, 1, "13551505", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 6%", true, false, false, false, "M", null },
                    { 10074, true, 1, "13551506", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución retención en la fuente 6%", true, false, false, false, "M", null },
                    { 10075, true, 1, "13551507", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 10%", true, false, false, false, "M", null },
                    { 10076, true, 1, "13551508", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Retención en la fuente 10%", true, false, false, false, "M", null },
                    { 10077, true, 1, "13551509", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 11%", true, false, false, false, "M", null },
                    { 10078, true, 1, "13551510", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución en ventas Retefuente 11%", true, false, false, false, "M", null },
                    { 10079, true, 1, "13551511", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 7%", true, false, false, false, "M", null },
                    { 10080, true, 1, "13551512", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Retención en la fuente 7%", true, false, false, false, "M", null },
                    { 10081, true, 1, "13551513", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 3,5%", true, false, false, false, "M", null },
                    { 10082, true, 1, "13551514", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Retención en la fuente 3,5%", true, false, false, false, "M", null },
                    { 10083, true, 1, "13551515", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 2%", true, false, false, false, "M", null },
                    { 10084, true, 1, "13551516", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Retención en la fuente 2%", true, false, false, false, "M", null },
                    { 10085, true, 1, "13551517", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo Retención en la fuente 1%", true, false, false, false, "M", null },
                    { 10086, true, 1, "13551518", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Retención en la fuente 1%", true, false, false, false, "M", null },
                    { 10087, true, 1, "13551519", "135515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Autorretención", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10088, 1, "135517", "1355", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Impuesto a las ventas retenido", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10089, true, 1, "13551701", "135517", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto a las ventas retenido 15%", true, false, false, false, "M", null },
                    { 10090, true, 1, "13551702", "135517", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución impuesto a las ventas retenido 15%", true, false, false, false, "M", null },
                    { 10091, true, 1, "13551703", "135517", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto a las ventas retenido 100%", true, false, false, false, "M", null },
                    { 10092, true, 1, "13551704", "135517", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Impuesto a las ventas retenido 100%", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10093, 1, "13551797", "135517", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal impuesto a las ventas retenido", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10094, 1, "135518", "1355", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Impuesto de industria y comercio retenido", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10095, true, 1, "13551801", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete Ica 11,04", true, false, false, false, "M", null },
                    { 10096, true, 1, "13551802", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Rete Ica 11,04", true, false, false, false, "M", null },
                    { 10097, true, 1, "13551803", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete Ica 13,08", true, false, false, false, "M", null },
                    { 10098, true, 1, "13551804", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Rete Ica 13,08", true, false, false, false, "M", null },
                    { 10099, true, 1, "13551805", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete Ica 9,66", true, false, false, false, "M", null },
                    { 10100, true, 1, "13551806", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Rete Ica 9,66", true, false, false, false, "M", null },
                    { 10101, true, 1, "13551807", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete Ica 8", true, false, false, false, "M", null },
                    { 10102, true, 1, "13551808", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Rete Ica 8", true, false, false, false, "M", null },
                    { 10103, true, 1, "13551809", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete Ica 7", true, false, false, false, "M", null },
                    { 10104, true, 1, "13551810", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Rete Ica 7", true, false, false, false, "M", null },
                    { 10105, true, 1, "13551811", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete Ica 6.9", true, false, false, false, "M", null },
                    { 10106, true, 1, "13551812", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Rete Ica 6.9", true, false, false, false, "M", null },
                    { 10107, true, 1, "13551813", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rete Ica 4", true, false, false, false, "M", null },
                    { 10108, true, 1, "13551814", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Devolución Rete Ica 4", true, false, false, false, "M", null },
                    { 10109, true, 1, "13551815", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto de industria y comercio retenido", true, false, false, false, "M", null },
                    { 10110, true, 1, "13551816", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Autorretencion impuesto de industria y comercio", true, false, false, false, "M", null },
                    { 10111, true, 1, "13551817", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Autorretencion avisos y tableros", true, false, false, false, "M", null },
                    { 10112, true, 1, "13551818", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo de impuesto de industria y comercio", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10113, 1, "13551897", "135518", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal Impuesto de industria y comercio retenido", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10114, 1, "1365", "13", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Cuentas por cobrar a trabajadores", false, false, false, "M", null },
                    { 10115, 1, "136515", "1365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Educación", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10116, true, 1, "13651501", "136515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Educación", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10117, 1, "13651597", "136515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal educación", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10118, 1, "136525", "1365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Calamidad domestica", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10119, true, 1, "13652501", "136525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Calamidad domestica", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10120, 1, "13652597", "136525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal calamidad domestica", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10121, 1, "1380", "13", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Deudores varios", false, false, false, "M", null },
                    { 10122, 1, "138095", "1380", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10123, true, 1, "13809501", "138095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Incapacidades", true, false, false, false, "M", null },
                    { 10124, true, 1, "13809502", "138095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otros deudores", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10125, 1, "13809597", "138095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal otros deudores", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10126, 1, "1398", "13", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Causacin automtica ventas (sistema)", false, false, false, "M", null },
                    { 10127, 1, "1399", "13", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Provisiones", false, false, false, "M", null },
                    { 10128, 1, "139905", "1399", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Clientes", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10129, true, 1, "13990501", "139905", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Clientes", true, false, false, false, "M", null },
                    { 10130, true, 1, "13990597", "139905", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal clientes", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10131, 1, "14", "1", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Inventarios", false, false, false, "M", null },
                    { 10132, 1, "1435", "14", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Mercancías no fabricadas por la empresa", false, false, false, "M", null },
                    { 10133, 1, "143501", "1435", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Mercancías no fabricadas", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10134, true, 1, "14350101", "143501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Mercancías no fabricadas", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10135, 1, "14350197", "143501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal mercancías no fabricadas", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10136, 1, "1498", "14", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Otros", false, false, false, "M", null },
                    { 10137, 1, "149801", "1498", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10138, true, 1, "14980101", "149801", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10139, 1, "14980197", "149801", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal Otros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10140, 1, "15", "1", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Propiedad planta y equipo", false, false, false, "M", null },
                    { 10141, 1, "1504", "15", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Terrenos", false, false, false, "M", null },
                    { 10142, 1, "150405", "1504", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Urbanos", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10143, true, 1, "15040505", "150405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Urbanos", true, false, false, false, "M", null },
                    { 10144, true, 1, "15040597", "150405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal revaluación urbanos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10145, 1, "1516", "15", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Construcciones y edificaciones", false, false, false, "M", null },
                    { 10146, 1, "151605", "1516", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Edificios", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10147, true, 1, "15160501", "151605", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Edificios", true, false, false, false, "M", null },
                    { 10148, true, 1, "15160597", "151605", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal edificios", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10149, 1, "151610", "1516", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Edificios", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10150, true, 1, "15161001", "151610", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Oficinas", true, false, false, false, "M", null },
                    { 10151, true, 1, "15161097", "151610", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal revaluación construcciones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10152, 1, "1524", "15", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Equipo de oficina", false, false, false, "M", null },
                    { 10153, 1, "152405", "1524", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Muebles y enseres", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10154, true, 1, "15240501", "152405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Muebles y enseres", true, false, false, false, "M", null },
                    { 10155, true, 1, "15240597", "152405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal muebles y enseres", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10156, 1, "152410", "1524", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Equipos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10157, true, 1, "15241001", "152410", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Equipos", true, false, false, false, "M", null },
                    { 10158, true, 1, "15241097", "152410", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal equipos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10159, 1, "1528", "15", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Equipo de computación y comunicación", false, false, false, "M", null },
                    { 10160, 1, "152805", "1528", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Equipos de procesamiento de datos", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10161, true, 1, "15280501", "152805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Equipos de procesamiento de datos", true, false, false, false, "M", null },
                    { 10162, true, 1, "15280597", "152805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal equipos de procesamiento de datos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10163, 1, "1540", "15", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Flota y equipo de transporte", false, false, false, "M", null },
                    { 10164, 1, "154005", "1540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Vehículos en leasing", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10165, true, 1, "15400501", "154005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Vehículos en leasing", true, false, false, false, "M", null },
                    { 10166, true, 1, "15400597", "154005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal vehículos leasing", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10167, 1, "1592", "15", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Depreciación acumulada", false, false, false, "M", null },
                    { 10168, 1, "159205", "1592", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Construcciones y edificaciones", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10169, true, 1, "15920501", "159205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Construcciones y edificaciones", true, false, false, false, "M", null },
                    { 10170, true, 1, "15920597", "159205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal valor de salvamento construcciones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10171, 1, "159215", "1592", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Equipo de oficina", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10172, true, 1, "15921501", "159215", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Equipo de oficina", true, false, false, false, "M", null },
                    { 10173, true, 1, "15921597", "159215", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal equipo de oficina", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10174, 1, "159220", "1592", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Equipo de computación y comunicación", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10175, true, 1, "15922001", "159220", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Equipo de computación y comunicación", true, false, false, false, "M", null },
                    { 10176, true, 1, "15922097", "159220", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal equipo de computación y comunicación", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10177, 1, "159235", "1592", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Flota y equipo de transporte", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10178, true, 1, "15923501", "159235", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Flota y equipo de transporte", true, false, false, false, "M", null },
                    { 10179, true, 1, "15923597", "159235", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal depreciación flota", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10180, 1, "16", "1", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Intangibles", false, false, false, "M", null },
                    { 10181, 1, "1635", "16", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Licencias", false, false, false, "M", null },
                    { 10182, 1, "163501", "1635", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Derecho de uso", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10183, true, 1, "16350101", "163501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Derecho de uso", true, false, false, false, "M", null },
                    { 10184, true, 1, "16350197", "163501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal derecho de uso", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10185, 1, "163515", "1635", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Marca adquirida", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10186, true, 1, "16351501", "163515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Marca adquirida", true, false, false, false, "M", null },
                    { 10187, true, 1, "16351597", "163515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal marca adquirida", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10188, 1, "17", "1", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Otros activos no financieros", false, false, false, "M", null },
                    { 10189, 1, "1720", "17", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Entidades controladas en forma conjunta", false, false, false, "M", null },
                    { 10190, 1, "172020", "1720", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Negocios conjuntos", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10191, true, 1, "17202001", "172020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Negocios conjuntos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10192, 1, "17202097", "172020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal Negocios conjuntos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10193, 1, "18", "1", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Impuesto a las ganancias", false, false, false, "M", null },
                    { 10194, 1, "1805", "18", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Impuesto corriente", false, false, false, "M", null },
                    { 10195, 1, "180505", "1805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Renta y complementarios", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10196, true, 1, "18050501", "180505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Autorretencion servicios", true, false, false, false, "M", null },
                    { 10197, true, 1, "18050502", "180505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Retención en la fuente compras 1.5%", true, false, false, false, "M", null },
                    { 10198, true, 1, "18050503", "180505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Tarjetas de crédito", true, false, false, false, "M", null },
                    { 10199, true, 1, "18050504", "180505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Servicios 6%", true, false, false, false, "M", null },
                    { 10200, true, 1, "18050505", "180505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Autorretencion", true, false, false, false, "M", null },
                    { 10201, true, 1, "18050506", "180505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Anticipo sobretasa cree", true, false, false, false, "M", null },
                    { 10202, true, 1, "18050507", "180505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Sobrantes en liquidación privada de impuestos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10203, 1, "18050597", "180505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal sobrantes en liquidación privada de impuestos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10204, 1, "19", "1", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Otros activos financieros", false, false, false, "M", null },
                    { 10205, 1, "1945", "19", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "De inversiones", false, false, false, "M", null },
                    { 10206, 1, "194510", "1945", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "De inversiones", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10207, true, 1, "19451001", "194510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "De inversiones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10208, 1, "19451097", "194510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal de inversiones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10209, 2, "2", null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 1, "Pasivo", false, false, false, "M", null },
                    { 10210, 2, "21", "2", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Pasivos financieros", false, false, false, "M", null },
                    { 10211, 2, "2105", "21", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Bancos nacionales", false, false, false, "M", null },
                    { 10212, 2, "210510", "2105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Pagares", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10213, true, 2, "21051001", "210510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Colpatria costo amortizado", true, false, false, false, "M", null },
                    { 10214, true, 2, "21051002", "210510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "BBVA diferencia certificado", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10215, 2, "21051097", "210510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal BBVA diferencia certificado", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10216, 2, "2110", "21", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Depósitos recibidos", false, false, false, "M", null },
                    { 10217, 2, "211095", "2110", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Otros", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10218, true, 2, "21109501", "211095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Otros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10219, 2, "21109597", "211095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal otros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10220, 2, "22", "2", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Proveedores", false, false, false, "M", null },
                    { 10221, 2, "2205", "22", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Proveedores nacionales", false, false, false, "M", null },
                    { 10222, 2, "220505", "2205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Proveedores nacionales", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10223, true, 2, "22050501", "220505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Proveedores nacionales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10224, 2, "22050597", "220505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal proveedores nacionales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10225, 2, "2210", "22", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Proveedores del exterior", false, false, false, "M", null },
                    { 10226, 2, "221005", "2210", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Proveedores del exterior", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10227, true, 2, "22100501", "221005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Proveedores del exterior", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10228, 2, "22100597", "221005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal proveedores del exterior", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10229, 2, "2299", "22", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Causación automática compras (sistema)", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10230, true, 2, "229999", "2299", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Causación automática compras (sistema)", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10231, 2, "23", "2", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Acreedores comerciales y otras cuentas por pagar", false, false, false, "M", null },
                    { 10232, 2, "2305", "23", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Cuentas corrientes comerciales", false, false, false, "M", null },
                    { 10233, 2, "230505", "2305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Cuentas corrientes comerciales", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10234, true, 2, "23050501", "230505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Cuentas corrientes comerciales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10235, 2, "23050597", "230505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal cuentas corrientes comerciales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10236, 2, "2335", "23", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Costos y gastos por pagar", false, false, false, "M", null },
                    { 10237, 2, "233525", "2335", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Honorarios", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10238, true, 2, "23352501", "233525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Honorarios", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10239, 2, "23352597", "233525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal honorarios", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10240, 2, "233595", "2335", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Otros", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10241, true, 2, "23359501", "233595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Otros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10242, 2, "23359597", "233595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal Otros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10243, 2, "2365", "23", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Retenciones en la fuente", false, false, false, "M", null },
                    { 10244, 2, "236505", "2365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Salarios y pagos laborales", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10245, true, 2, "23650501", "236505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Salarios y pagos laborales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10246, 2, "23650597", "236505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal salarios y pagos laborales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10247, 2, "236510", "2365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Dividendos y/o participaciones", false, false, false, "M", null },
                    { 10248, 2, "236515", "2365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Honorarios", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10249, true, 2, "23651501", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Honorarios", true, false, false, false, "M", null },
                    { 10250, true, 2, "23651502", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Honorarios", true, false, false, false, "M", null },
                    { 10251, true, 2, "23651503", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Retención 7%", true, false, false, false, "M", null },
                    { 10252, true, 2, "23651504", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución retención 7%", true, false, false, false, "M", null },
                    { 10253, true, 2, "23651505", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Retención 3,5%", true, false, false, false, "M", null },
                    { 10254, true, 2, "23651506", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución retención 3,5%", true, false, false, false, "M", null },
                    { 10255, true, 2, "23651507", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Retención 2%", true, false, false, false, "M", null },
                    { 10256, true, 2, "23651508", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución retención 2%", true, false, false, false, "M", null },
                    { 10257, true, 2, "23651509", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Retención 1%", true, false, false, false, "M", null },
                    { 10258, true, 2, "23651510", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución retención 1%", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10259, 2, "23651597", "236515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal devolución Honorarios", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10260, 2, "236520", "2365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Comisiones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10261, true, 2, "23652001", "236520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Comisiones", true, false, false, false, "M", null },
                    { 10262, true, 2, "23652002", "236520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Comisiones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10263, 2, "23652097", "236520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal comisiones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10264, 2, "236525", "2365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Servicios", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10265, true, 2, "23652501", "236525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Servicios 6%", true, false, false, false, "M", null },
                    { 10266, true, 2, "23652502", "236525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Servicios 6%", true, false, false, false, "M", null },
                    { 10267, true, 2, "23652503", "236525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Servicios 4%", true, false, false, false, "M", null },
                    { 10268, true, 2, "23652504", "236525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Servicios 4%", true, false, false, false, "M", null },
                    { 10269, true, 2, "23652505", "236525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Servicios  1 %", true, false, false, false, "M", null },
                    { 10270, true, 2, "23652506", "236525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución servicios  1 %", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10271, 2, "23652597", "236525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal servicios", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10272, 2, "236530", "2365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Arrendamientos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10273, true, 2, "23653001", "236530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Arrendamientos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10274, 2, "23653097", "236530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal arrendamientos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10275, 2, "236535", "2365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Rendimientos financieros", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10276, true, 2, "23653501", "236535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Rendimientos financieros", true, false, false, false, "M", null },
                    { 10277, true, 2, "23653502", "236535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Rendimientos financieros 7 %", true, false, false, false, "M", null },
                    { 10278, true, 2, "23653503", "236535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución rendimientos financieros 7 %", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10279, 2, "23653597", "236535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal rendimientos financieros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10280, 2, "236540", "2365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Retención por compras", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10281, true, 2, "23654001", "236540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Retención por compras 2,5%", true, false, false, false, "M", null },
                    { 10282, true, 2, "23654002", "236540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Retención por compras 2,5%", true, false, false, false, "M", null },
                    { 10283, true, 2, "23654004", "236540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Retención por compras 3,5%", true, false, false, false, "M", null },
                    { 10284, true, 2, "23654005", "236540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Retención por compras 3,5%", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10285, 2, "23654097", "236540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal retención por compras", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10286, 2, "236570", "2365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Otras retenciones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10287, true, 2, "23657001", "236570", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Otras retenciones  2 %", true, false, false, false, "M", null },
                    { 10288, true, 2, "23657002", "236570", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución otras retenciones  2 %", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10289, 2, "236575", "2365", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Autorretenciones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10290, true, 2, "23657501", "236575", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Autorretenciones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10291, 2, "23657597", "236575", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal Autorretenciones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10292, 2, "2367", "23", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Impuesto a las ventas retenido", false, false, false, "M", null },
                    { 10293, 2, "236701", "2367", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Impuesto a las ventas retenido", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10294, true, 2, "23670101", "236701", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto a las ventas retenido 15%", true, false, false, false, "M", null },
                    { 10295, true, 2, "23670102", "236701", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Impuesto a las ventas retenido 15%", true, false, false, false, "M", null },
                    { 10296, true, 2, "23670103", "236701", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto a las ventas retenido 100%", true, false, false, false, "M", null },
                    { 10297, true, 2, "23670104", "236701", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Impuesto a las ventas retenido 100%", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10298, 2, "23670197", "236701", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal impuesto a las ventas retenido", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10299, 2, "236705", "2367", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Retención de impuesto a las ventas Iva", false, false, false, "M", null },
                    { 10300, 2, "236768", "2367", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Impuesto a las ventas retenido", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10301, true, 2, "23676801", "236768", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto a las ventas retenido", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10302, 2, "23676897", "236768", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal retención de impuesto a las ventas Iva", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10303, 2, "2368", "23", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Impuesto de industria y comercio retenido", false, false, false, "M", null },
                    { 10304, 2, "236805", "2368", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Retención industria y comercio Ica", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10305, true, 2, "23680501", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Reteica 11,04", true, false, false, false, "M", null },
                    { 10306, true, 2, "23680502", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Reteica 11,04", true, false, false, false, "M", null },
                    { 10307, true, 2, "23680503", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Reteica 13,8", true, false, false, false, "M", null },
                    { 10308, true, 2, "23680504", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Reteica 13,8", true, false, false, false, "M", null },
                    { 10309, true, 2, "23680505", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Reteica 9,66", true, false, false, false, "M", null },
                    { 10310, true, 2, "23680506", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Reteica 9,66", true, false, false, false, "M", null },
                    { 10311, true, 2, "23680507", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Reteica 8", true, false, false, false, "M", null },
                    { 10312, true, 2, "23680508", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Reteica 8", true, false, false, false, "M", null },
                    { 10313, true, 2, "23680509", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Reteica 7", true, false, false, false, "M", null },
                    { 10314, true, 2, "23680510", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Reteica 7", true, false, false, false, "M", null },
                    { 10315, true, 2, "23680511", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Reteica 6,9", true, false, false, false, "M", null },
                    { 10316, true, 2, "23680512", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Reteica 6,9", true, false, false, false, "M", null },
                    { 10317, true, 2, "23680513", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Reteica 4,14", true, false, false, false, "M", null },
                    { 10318, true, 2, "23680514", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución Reteica 4,14", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10319, 2, "23680597", "236805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal retención industria y comercio", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10320, 2, "2370", "23", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Aportes a empresas promotoras de salud eps", false, false, false, "M", null },
                    { 10321, 2, "237005", "2370", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Aportes a entidades promotoras de salud eps", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10322, true, 2, "23700501", "237005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Aportes a entidades promotoras de salud eps", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10323, 2, "23700597", "237005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal aportes a entidades promotoras de salud eps", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10324, 2, "237006", "2370", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Aporte a administradoras de riesgos profesionales", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10325, true, 2, "23700601", "237006", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Aporte a administradoras de riesgos profesionales, ARL", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10326, 2, "237010", "2370", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Aportes al icbf Sena y cajas de compensación", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10327, true, 2, "23701001", "237010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Aportes al icbf Sena y cajas de compensación", true, false, false, false, "M", null },
                    { 10328, true, 2, "23701002", "237010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Sena", true, false, false, false, "M", null },
                    { 10329, true, 2, "23701003", "237010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Icbf", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10330, 2, "23701097", "237010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal aportes al icbf Sena y cajas de compensación", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10331, 2, "237015", "2370", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Aportes arl", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10332, true, 2, "23701501", "237015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Aportes arl", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10333, 2, "23701597", "237015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal aportes arl", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10334, 2, "237025", "2370", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Embargos judiciales", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10335, true, 2, "23702501", "237025", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Embargos judiciales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10336, 2, "23702597", "237025", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal embargos judiciales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10337, 2, "237030", "2370", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Libranzas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10338, true, 2, "23703001", "237030", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Libranzas", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10339, 2, "23703097", "237030", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal Libranzas", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10340, 2, "237045", "2370", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Fondos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10341, true, 2, "23704501", "237045", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Fondos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10342, 2, "23704597", "237045", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal fondos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10343, 2, "237050", "2370", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Ahorro afc", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10344, true, 2, "23705001", "237050", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Ahorro afc", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10345, 2, "23705097", "237050", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal ahorro afc", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10346, 2, "2380", "23", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Acreedores varios", false, false, false, "M", null },
                    { 10347, 2, "238030", "2380", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Fondos de cesantías y/o pensiones", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10348, true, 2, "23803001", "238030", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Fondos de cesantías y/o pensiones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10349, 2, "23803097", "238030", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal Fondos de cesantías y/o pensiones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10350, 2, "238095", "2380", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Otros", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10351, true, 2, "23809501", "238095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Otros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10352, 2, "23809597", "238095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal otros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10353, 2, "24", "2", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Pasivos por impuestos", false, false, false, "M", null },
                    { 10354, 2, "2404", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "De renta y complementarios corriente", false, false, false, "M", null },
                    { 10355, 2, "240405", "2404", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Vigencia fiscal corriente", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10356, true, 2, "24040501", "240405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Vigencia fiscal corriente", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10357, 2, "24040597", "240405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal vigencia fiscal corriente", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10358, 2, "2408", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Impuesto sobre las ventas por pagar", false, false, false, "M", null },
                    { 10359, 2, "240805", "2408", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Iva generado en ventas", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10360, true, 2, "24080501", "240805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Iva generado en ventas", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10361, 2, "240806", "2408", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Iva generado", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10362, true, 2, "24080601", "240806", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Iva generado", true, false, false, false, "M", null },
                    { 10363, true, 2, "24080602", "240806", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Iva generado", true, false, false, false, "M", null },
                    { 10364, true, 2, "24080603", "240806", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Iva generado 16%", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10365, 2, "240810", "2408", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Iva descontable por compras", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10366, true, 2, "24081001", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Iva descontable por compras 19%", true, false, false, false, "M", null },
                    { 10367, true, 2, "24081002", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Iva Devolución en compras 19%", true, false, false, false, "M", null },
                    { 10368, true, 2, "24081003", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Iva descontable por compras 5%", true, false, false, false, "M", null },
                    { 10369, true, 2, "24081004", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Iva Devolución en compras 5%", true, false, false, false, "M", null },
                    { 10370, true, 2, "24081005", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Compras iva 16%", true, false, false, false, "M", null },
                    { 10371, true, 2, "24081006", "240810", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución de compra 16%", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10372, 2, "240815", "2408", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Descontable por servicios", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10373, true, 2, "24081501", "240815", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Descontable por servicios", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10374, 2, "240820", "2408", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Descontable por devoluciones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10375, true, 2, "24082001", "240820", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Descontable por devoluciones 19%", true, false, false, false, "M", null },
                    { 10376, true, 2, "24082002", "240820", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Descontable por devoluciones 5%", true, false, false, false, "M", null },
                    { 10377, true, 2, "24082003", "240820", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución en venta 16 %", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10378, 2, "240830", "2408", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Descontable régimen simplificado", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10379, true, 2, "24083001", "240830", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Descontable régimen simplificado", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10380, 2, "2464", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "De licores, cervezas y cigarrillos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10381, true, 2, "246401", "2464", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Impuesto por valor en ventas", true, false, false, false, "M", null },
                    { 10382, true, 2, "246402", "2464", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Impuesto por valor en devolucion en ventas", true, false, false, false, "M", null },
                    { 10383, true, 2, "246403", "2464", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Impuesto por valor en compras", true, false, false, false, "M", null },
                    { 10384, true, 2, "246404", "2464", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Impuesto por valor en devolucion en compras", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10385, 2, "246405", "2464", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Impuesto Ad valorem en ventas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10386, true, 2, "24640501", "246405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto Ad valorem en ventas 20%", true, false, false, false, "M", null },
                    { 10387, true, 2, "24640502", "246405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto Ad valorem en ventas 25%", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10388, 2, "246406", "2464", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Impuesto Ad valorem en devolución en ventas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10389, true, 2, "24640601", "246406", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto Ad valorem en devoluciones ventas 20%", true, false, false, false, "M", null },
                    { 10390, true, 2, "24640602", "246406", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto Ad valorem en devoluciones ventas 25%", true, false, false, false, "M", null },
                    { 10391, true, 2, "24651005", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Ventas - Comestibles ultraprocesados 15", true, false, false, false, "M", null },
                    { 10392, true, 2, "24651006", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución ventas - Comestibles ultraprocesados 15", true, false, false, false, "M", null },
                    { 10393, true, 2, "24651007", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Compras - Comestibles ultraprocesados 15", true, false, false, false, "M", null },
                    { 10394, true, 2, "24651008", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución compras - Comestibles ultraprocesados 15", true, false, false, false, "M", null },
                    { 10395, true, 2, "24651009", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Ventas - Comestibles ultraprocesados 20", true, false, false, false, "M", null },
                    { 10396, true, 2, "24651010", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución ventas - Comestibles ultraprocesados 20", true, false, false, false, "M", null },
                    { 10397, true, 2, "24651011", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Compras - Comestibles ultraprocesados 20", true, false, false, false, "M", null },
                    { 10398, true, 2, "24651012", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución compras - Comestibles ultraprocesados 20", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10399, 2, "2495", "24", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Otros", false, false, false, "M", null },
                    { 10400, 2, "249501", "2495", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Impuesto al consumo nacional", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10401, true, 2, "24950101", "249501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto al consumo en ventas", true, false, false, false, "M", null },
                    { 10402, true, 2, "24950102", "249501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto al consumo en devolucion en ventas", true, false, false, false, "M", null },
                    { 10403, true, 2, "24950103", "249501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto al consumo en compras", true, false, false, false, "M", null },
                    { 10404, true, 2, "24950104", "249501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Impuesto al consumo en devolucion en compras", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10405, 2, "25", "2", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Beneficios a empleados", false, false, false, "M", null },
                    { 10406, 2, "2505", "25", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Salarios por pagar", false, false, false, "M", null },
                    { 10407, 2, "250505", "2505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Salarios por pagar", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10408, true, 2, "25050501", "250505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Salarios por pagar", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10409, 2, "25050597", "250505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal salarios por pagar", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10410, 2, "2510", "25", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Pasivo estimado para obligaciones laborales", false, false, false, "M", null },
                    { 10411, 2, "251010", "2510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Pasivo estimado para obligaciones laborales", false, false, false, "M", null },
                    { 10412, 2, "25101001", "251010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Cesantías", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10413, true, 2, "2510100101", "25101001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 6, "Cesantías", true, false, false, false, "M", null },
                    { 10414, true, 2, "2510100102", "25101001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 6, "Cesantías", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10415, 2, "25101002", "251010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Intereses sobre cesantías", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10416, true, 2, "2510100201", "25101002", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 6, "Intereses sobre cesantías", true, false, false, false, "M", null },
                    { 10417, true, 2, "2510100202", "25101002", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 6, "Intereses sobre cesantías", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10418, 2, "25101003", "251010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Vacaciones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10419, true, 2, "2510100301", "25101003", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 6, "Vacaciones", true, false, false, false, "M", null },
                    { 10420, true, 2, "2510100302", "25101003", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 6, "Vacaciones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10421, 2, "25101004", "251010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Prima de servicios", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10422, true, 2, "2510100401", "25101004", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 6, "Prima de servicios", true, false, false, false, "M", null },
                    { 10423, true, 2, "2510100402", "25101004", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 6, "Prima de servicios", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10424, 2, "25101097", "251010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal pasivo estimado para obligaciones laborales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10425, 2, "28", "2", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Pasivos no financieros", false, false, false, "M", null },
                    { 10426, 2, "2805", "28", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Anticipos y avances recibidos", false, false, false, "M", null },
                    { 10427, 2, "280505", "2805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "De clientes", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10428, true, 2, "28050501", "280505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "De clientes", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10429, 2, "28050597", "280505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal de clientes", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10430, 2, "2815", "28", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Ingresos recibidos para terceros", false, false, false, "M", null },
                    { 10431, 2, "281505", "2815", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Valores recibidos para terceros", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10432, true, 2, "28150501", "281505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Valores recibidos para terceros", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10433, 2, "28150597", "281505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal fondo innovación presidencia calidad", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10434, 3, "3", null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 1, "Patrimonio", false, false, false, "M", null },
                    { 10435, 3, "31", "3", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Capital social", false, false, false, "M", null },
                    { 10436, 3, "3105", "31", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Capital suscrito y pagado", false, false, false, "M", null },
                    { 10437, 3, "310505", "3105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Capital suscrito y pagado", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10438, true, 3, "31050501", "310505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Capital autorizado", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10439, 3, "310510", "3105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Capital por suscribir (db)", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10440, true, 3, "31051001", "310510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Capital por suscribir (db)", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10441, 3, "32", "3", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Superávit de capital", false, false, false, "M", null },
                    { 10442, 3, "3205", "32", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Prima en colocación de acciones cuotas o partes d", false, false, false, "M", null },
                    { 10443, 3, "320505", "3205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Prima en colocación de acciones", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10444, true, 3, "32050501", "320505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Prima en colocación de acciones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10445, 3, "320520", "3205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Superávit por el método de participación", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10446, true, 3, "32052001", "320520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Superávit por el método de participación", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10447, 3, "33", "3", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Reservas", false, false, false, "M", null },
                    { 10448, 3, "3305", "33", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Reservas obligatorias", false, false, false, "M", null },
                    { 10449, 3, "330505", "3305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Reservas obligatorias", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10450, true, 3, "33050501", "330505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Reserva legal", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10451, 3, "36", "3", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Resultado del ejercicio", false, false, false, "M", null },
                    { 10452, 3, "3605", "36", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Utilidad del ejercicio", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10453, true, 3, "360505", "3605", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Utilidad del ejercicio", true, false, false, false, "M", null },
                    { 10454, true, 3, "360597", "3605", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Utilidad del ejercicio Fiscal", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10455, 3, "3610", "36", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Perdida del ejercicio", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10456, true, 3, "361005", "3610", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Perdida del ejercicio", true, false, false, false, "M", null },
                    { 10457, true, 3, "361097", "3610", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Perdida del ejercicio Fiscal", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10458, 3, "37", "3", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Resultados de ejercicios anteriores", false, false, false, "M", null },
                    { 10459, 3, "3705", "37", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Resultados de ejercicios anteriores", false, false, false, "M", null },
                    { 10460, 3, "370505", "3705", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Resultados de ejercicios anteriores", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10461, true, 3, "37050501", "370505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Utilidades o excedentes acumulados", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10462, 3, "3710", "37", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Convergencia", false, false, false, "M", null },
                    { 10463, 3, "371005", "3710", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Convergencia", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10464, true, 3, "37100501", "371005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Otros aportes de capital", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10465, 3, "39", "3", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Afectaciones fiscales de ingresos y gastos", false, false, false, "M", null },
                    { 10466, 3, "3905", "39", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Resultados fiscales de ventas en ganancia ocasional", false, false, false, "M", null },
                    { 10467, 3, "390505", "3905", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Resultados fiscales de ventas en ganancia ocasional", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10468, true, 3, "39050501", "390505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Resultados fiscales de ventas en ganancia ocasional", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10469, 4, "4", null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 1, "Ingresos", false, false, false, "M", null },
                    { 10470, 4, "41", "4", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Ingresos de actividades ordinarias", false, false, false, "M", null },
                    { 10471, 4, "4135", "41", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Comercio al por mayor y al detal", false, false, false, "M", null },
                    { 10472, 4, "413501", "4135", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Comercio al por mayor y al detal", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10473, true, 4, "41350101", "413501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Comercio al por mayor y al detal", true, false, false, false, "M", null },
                    { 10474, true, 4, "41350102", "413501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Descuentos", true, false, false, false, "M", null },
                    { 10475, true, 4, "41350197", "413501", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal Comercio al por mayor y al detal", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10476, 4, "4175", "41", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Devolución en ventas", false, false, false, "M", null },
                    { 10477, 4, "417505", "4175", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Devolución", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10478, true, 4, "41750501", "417505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución en ventas", true, false, false, false, "M", null },
                    { 10479, true, 4, "41750502", "417505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución en servicios", true, false, false, false, "M", null },
                    { 10480, true, 4, "41750503", "417505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Devolución en descuentos", true, false, false, false, "M", null },
                    { 10481, true, 4, "41750597", "417505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal devolución en ventas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10482, 4, "4180", "41", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Servicios", false, false, false, "M", null },
                    { 10483, 4, "418001", "4180", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Servicios", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10484, true, 4, "41800101", "418001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Servicios", true, false, false, false, "M", null },
                    { 10485, true, 4, "41800102", "418001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Descuentos en servicios", true, false, false, false, "M", null },
                    { 10486, true, 4, "41800197", "418001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal servicios", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10487, 4, "42", "4", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Otros ingresos de actividades ordinarias", false, false, false, "M", null },
                    { 10488, 4, "4210", "42", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Financieros", false, false, false, "M", null },
                    { 10489, 4, "421020", "4210", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Diferencia en cambio", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10490, true, 4, "42102001", "421020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Diferencia en cambio", true, false, false, false, "M", null },
                    { 10491, true, 4, "42102097", "421020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal diferencia en cambio", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10492, 4, "421040", "4210", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Descuentos comerciales condicionados", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10493, true, 4, "42104001", "421040", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Descuentos comerciales condicionados", true, false, false, false, "M", null },
                    { 10494, true, 4, "42104097", "421040", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal descuentos comerciales condicionados", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10495, 4, "4218", "42", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Ingresos método de participación", false, false, false, "M", null },
                    { 10496, 4, "421805", "4218", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "De sociedades anónimas y/o asimiladas", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10497, true, 4, "42180501", "421805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "De sociedades anónimas y/o asimiladas", true, false, false, false, "M", null },
                    { 10498, true, 4, "42180597", "421805", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal de sociedades anónimas y/o asimiladas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10499, 4, "4295", "42", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Diversos", false, false, false, "M", null },
                    { 10500, 4, "429505", "4295", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Aprovechamientos", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10501, true, 4, "42950501", "429505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Aprovechamientos", true, false, false, false, "M", null },
                    { 10502, true, 4, "42950502", "429505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Otros ingresos", true, false, false, false, "M", null },
                    { 10503, true, 4, "42950597", "429505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal aprovechamientos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10504, 4, "429581", "4295", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Ajuste al peso", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10505, true, 4, "42958101", "429581", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Ajuste al peso", true, false, false, false, "M", null },
                    { 10506, true, 4, "42958197", "429581", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal ajuste al peso", true, false, false, false, "M", null },
                    { 10507, true, 4, "429595", "4295", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Ingresos diversos POS", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10508, 4, "43", "4", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Ganancias", false, false, false, "M", null },
                    { 10509, 4, "4305", "43", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Propiedad planta y equipo", false, false, false, "M", null },
                    { 10510, 4, "430505", "4305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Revaluación", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10511, true, 4, "43050501", "430505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Revaluación de terrenos y edificaciones", true, false, false, false, "M", null },
                    { 10512, true, 4, "43050597", "430505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal revaluación de terrenos y edificaciones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10513, 4, "430510", "4305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Salvamento", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10514, true, 4, "43051001", "430510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Salvamento", true, false, false, false, "M", null },
                    { 10515, true, 4, "43051097", "430510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal salvamento", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10516, 4, "44", "4", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Ingresos fiscales", false, false, false, "M", null },
                    { 10517, 4, "4405", "44", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Ingresos por ganancia ocasional", false, false, false, "M", null },
                    { 10518, 4, "440505", "4405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Ingresos por ganancia ocasional", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10519, true, 4, "44050501", "440505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Ingresos por ganancia ocasional", true, false, false, false, "M", null },
                    { 10520, true, 4, "44050597", "440505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal Ingresos por ganancia ocasional", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10521, 4, "4410", "44", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Ingresos renta ordinaria", false, false, false, "M", null },
                    { 10522, 4, "441005", "4410", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Recuperación de deducciones fiscales", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10523, true, 4, "44100501", "441005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Recuperación de depreciación", true, false, false, false, "M", null },
                    { 10524, true, 4, "44100597", "441005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "D. fiscal recuperación de depreciación", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10525, 5, "5", null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 1, "Gasto", false, false, false, "M", null },
                    { 10526, 5, "51", "5", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Administrativos", false, false, false, "M", null },
                    { 10527, 5, "5105", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Gastos de personal", false, false, false, "M", null },
                    { 10528, 5, "510503", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Salario integral", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10529, true, 5, "51050301", "510503", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Salario integral", true, false, false, false, "M", null },
                    { 10530, true, 5, "51050397", "510503", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal salario integral", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10531, 5, "510506", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Sueldos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10532, true, 5, "51050601", "510506", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Sueldos", true, false, false, false, "M", null },
                    { 10533, true, 5, "51050697", "510506", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal sueldos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10534, 5, "510512", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Apoyo sostenimiento aprendices", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10535, true, 5, "51051201", "510512", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Apoyo sostenimiento aprendices", true, false, false, false, "M", null },
                    { 10536, true, 5, "51051297", "510512", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal apoyo sostenimiento aprendices", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10537, 5, "510515", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Horas extras y recargos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10538, true, 5, "51051501", "510515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Horas extras y recargos", true, false, false, false, "M", null },
                    { 10539, true, 5, "51051597", "510515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal horas extras y recargos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10540, 5, "510524", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Incapacidades", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10541, true, 5, "51052401", "510524", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Incapacidades", true, false, false, false, "M", null },
                    { 10542, true, 5, "51052497", "510524", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal incapacidades", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10543, 5, "510527", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Auxilio de transporte", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10544, true, 5, "51052701", "510527", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Auxilio de transporte", true, false, false, false, "M", null },
                    { 10545, true, 5, "51052797", "510527", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal auxilio de transporte", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10546, 5, "510530", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Cesantías", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10547, true, 5, "51053001", "510530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Cesantías", true, false, false, false, "M", null },
                    { 10548, true, 5, "51053097", "510530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal cesantías", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10549, 5, "510533", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Intereses sobre cesantías", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10550, true, 5, "51053301", "510533", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Intereses sobre cesantías", true, false, false, false, "M", null },
                    { 10551, true, 5, "51053397", "510533", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal intereses sobre cesantías", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10552, 5, "510536", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Prima de servicios", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10553, true, 5, "51053601", "510536", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Prima de servicios", true, false, false, false, "M", null },
                    { 10554, true, 5, "51053697", "510536", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal prima de servicios", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10555, 5, "510539", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Vacaciones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10556, true, 5, "51053901", "510539", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Vacaciones", true, false, false, false, "M", null },
                    { 10557, true, 5, "51053997", "510539", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal vacaciones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10558, 5, "510545", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Auxilios", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10559, true, 5, "51054501", "510545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Educativo", true, false, false, false, "M", null },
                    { 10560, true, 5, "51054502", "510545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Auxilio transporte", true, false, false, false, "M", null },
                    { 10561, true, 5, "51054597", "510545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal auxilio transporte", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10562, 5, "510548", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Bonificaciones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10563, true, 5, "51054801", "510548", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Bonificaciones", true, false, false, false, "M", null },
                    { 10564, true, 5, "51054897", "510548", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal bonificaciones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10565, 5, "510551", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Dotación y suministro a trabajadores", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10566, true, 5, "51055101", "510551", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Dotación y suministro a trabajadores", true, false, false, false, "M", null },
                    { 10567, true, 5, "51055197", "510551", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal dotación y suministro a trabajadores", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10568, 5, "510560", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Indemnizaciones laborales", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10569, true, 5, "51056001", "510560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Indemnizaciones laborales", true, false, false, false, "M", null },
                    { 10570, true, 5, "51056097", "510560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal indemnizaciones laborales", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10571, 5, "510563", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Capacitación al personal", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10572, true, 5, "51056301", "510563", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Capacitación al personal", true, false, false, false, "M", null },
                    { 10573, true, 5, "51056397", "510563", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal capacitación al personal", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10574, 5, "510566", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Gastos deportivos y de recreación", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10575, true, 5, "51056601", "510566", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos deportivos y de recreación", true, false, false, false, "M", null },
                    { 10576, true, 5, "51056697", "510566", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal gastos deportivos y de recreación", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10577, 5, "510568", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes a administradora de riesgos laborales", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10578, true, 5, "51056801", "510568", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes a administradora de riesgos laborales", true, false, false, false, "M", null },
                    { 10579, true, 5, "51056897", "510568", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes a administradora de riesgos laborales", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10580, 5, "510569", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes a entidades promotoras de salud eps", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10581, true, 5, "51056901", "510569", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes a entidades promotoras de salud eps", true, false, false, false, "M", null },
                    { 10582, true, 5, "51056997", "510569", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes a entidades promotoras de salud eps", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10583, 5, "510570", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aporte a fondos de pensión y/o cesantías", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10584, true, 5, "51057001", "510570", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aporte a fondos de pensión y/o cesantías", true, false, false, false, "M", null },
                    { 10585, true, 5, "51057097", "510570", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aporte a fondos de pensión y/o cesantías", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10586, 5, "510572", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes cajas de compensación familiar", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10587, true, 5, "51057201", "510572", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes cajas de compensación familiar", true, false, false, false, "M", null },
                    { 10588, true, 5, "51057297", "510572", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes cajas de compensación familiar", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10589, 5, "510575", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes icbf", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10590, true, 5, "51057501", "510575", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes icbf", true, false, false, false, "M", null },
                    { 10591, true, 5, "51057597", "510575", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes icbf", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10592, 5, "510578", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes Sena", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10593, true, 5, "51057801", "510578", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes Sena", true, false, false, false, "M", null },
                    { 10594, true, 5, "51057897", "510578", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes Sena", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10595, 5, "510584", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Gastos médicos y drogas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10596, true, 5, "51058401", "510584", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos médicos y drogas", true, false, false, false, "M", null },
                    { 10597, true, 5, "51058497", "510584", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal gastos médicos y drogas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10598, 5, "510595", "5105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10599, true, 5, "51059501", "510595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Bienestar y atención a empleados", true, false, false, false, "M", null },
                    { 10600, true, 5, "51059597", "510595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal bienestar y atención a empleados", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10601, 5, "5110", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Honorarios", false, false, false, "M", null },
                    { 10602, 5, "511010", "5110", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Revisoría fiscal", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10603, true, 5, "51101001", "511010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Honorarios - Revisoría fiscal", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10604, 5, "51101097", "511010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal revisoría fiscal", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10605, 5, "511015", "5110", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Auditoria externa", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10606, 5, "51101501", "511015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Auditoria externa", true, false, false, false, "M", null },
                    { 10607, 5, "51101597", "511015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal auditoria externa", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10608, 5, "511020", "5110", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Avalúos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10609, 5, "51102001", "511020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Avalúos", true, false, false, false, "M", null },
                    { 10610, 5, "51102097", "511020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal avalúos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10611, 5, "511025", "5110", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Asesoría jurídica", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10612, true, 5, "51102501", "511025", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Honorarios - Asesoría jurídica", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10613, 5, "51102597", "511025", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal asesoría jurídica", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10614, 5, "511035", "5110", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Asesoría técnica", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10615, 5, "51103501", "511035", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Asesoría técnica", true, false, false, false, "M", null },
                    { 10616, 5, "51103597", "511035", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal asesoría técnica", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10617, 5, "5115", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Impuestos", false, false, false, "M", null },
                    { 10618, 5, "511505", "5115", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Industria y comercio", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10619, 5, "51150501", "511505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Industria y comercio", true, false, false, false, "M", null },
                    { 10620, 5, "51150597", "511505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal industria y comercio", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10621, 5, "511515", "5115", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "A la propiedad raíz", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10622, 5, "51151501", "511515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "A la propiedad raíz", true, false, false, false, "M", null },
                    { 10623, 5, "51151597", "511515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal a la propiedad raíz", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10624, 5, "511540", "5115", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "De vehículos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10625, 5, "51154001", "511540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "De vehículos", true, false, false, false, "M", null },
                    { 10626, 5, "51154097", "511540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal impuesto de vehículos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10627, 5, "511570", "5115", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Prorrateo de Iva", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10628, 5, "51157001", "511570", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Prorrateo de Iva", true, false, false, false, "M", null },
                    { 10629, 5, "51157097", "511570", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal prorrateo de Iva", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10630, 5, "511595", "5115", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros impuestos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10631, 5, "51159501", "511595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otros impuestos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10632, true, 5, "51159502", "511595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto Ad valorem en compras 20%", true, false, false, false, "M", null },
                    { 10633, true, 5, "51159503", "511595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto Ad valorem en compras 25%", true, false, false, false, "M", null },
                    { 10634, true, 5, "51159504", "511595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto Ad valorem en devoluciones compras 20%", true, false, false, false, "M", null },
                    { 10635, true, 5, "51159505", "511595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto Ad valorem en devoluciones compras 25%", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10636, 5, "51159597", "511595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal otros impuesto", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10637, 5, "5120", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Arrendamientos", false, false, false, "M", null },
                    { 10638, 5, "512010", "5120", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Construcciones y edificaciones", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10639, true, 5, "51201001", "512010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Arrendamientos - Construcciones y edificaciones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10640, 5, "51201097", "512010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal construcciones y edificaciones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10641, 5, "512020", "5120", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Equipo de oficina", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10642, true, 5, "51202001", "512020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Arrendamientos - Equipo de oficina", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10643, 5, "51202097", "512020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal equipo de oficina", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10644, 5, "512025", "5120", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Equipo de computación", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10645, true, 5, "51202501", "512025", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Arrendamiento - Equipo de computación y comunicación", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10646, 5, "51202597", "512025", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal equipo de computación", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10647, 5, "512095", "5120", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Bodegaje", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10648, 5, "51209501", "512095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Bodegaje", true, false, false, false, "M", null },
                    { 10649, 5, "51209597", "512095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal bodegaje", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10650, 5, "5125", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Contribuciones y afiliaciones", false, false, false, "M", null },
                    { 10651, 5, "512510", "5125", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Afiliaciones y sostenimiento", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10652, 5, "51251001", "512510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Afiliaciones y sostenimiento", true, false, false, false, "M", null },
                    { 10653, 5, "51251097", "512510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal afiliaciones y sostenimiento", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10654, 5, "5130", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Seguros", false, false, false, "M", null },
                    { 10655, 5, "513010", "5130", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Cumplimiento", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10656, 5, "51301001", "513010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Cumplimiento", true, false, false, false, "M", null },
                    { 10657, 5, "51301097", "513010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal cumplimiento", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10658, 5, "513015", "5130", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Corriente débil", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10659, 5, "51301501", "513015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Corriente débil", true, false, false, false, "M", null },
                    { 10660, 5, "51301597", "513015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal corriente débil", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10661, 5, "513020", "5130", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Vida colectiva", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10662, 5, "51302001", "513020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Vida colectiva", true, false, false, false, "M", null },
                    { 10663, 5, "51302097", "513020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal vida colectiva", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10664, 5, "513030", "5130", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Terremoto", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10665, true, 5, "51303001", "513030", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Seguros - Terremoto", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10666, 5, "51303097", "513030", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal terremoto", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10667, 5, "513035", "5130", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Sustracción y hurto", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10668, 5, "51303501", "513035", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Sustracción y hurto", true, false, false, false, "M", null },
                    { 10669, 5, "51303597", "513035", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal sustracción y hurto", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10670, 5, "513040", "5130", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Flota y equipo de transporte", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10671, 5, "51304001", "513040", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Flota y equipo de transporte", true, false, false, false, "M", null },
                    { 10672, 5, "51304097", "513040", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal flota y equipo de transporte", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10673, 5, "513070", "5130", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Rotura de maquinaria", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10674, 5, "51307001", "513070", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Rotura de maquinaria", true, false, false, false, "M", null },
                    { 10675, 5, "51307097", "513070", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal rotura de maquinaria", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10676, 5, "513075", "5130", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Obligatorio accidente de transito", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10677, 5, "51307501", "513075", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Obligatorio accidente de transito", true, false, false, false, "M", null },
                    { 10678, 5, "51307597", "513075", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal obligatorio accidente de transito", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10679, 5, "5135", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Servicios", false, false, false, "M", null },
                    { 10680, 5, "513505", "5135", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aseo y vigilancia", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10681, true, 5, "51350501", "513505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Servicios - Aseo y vigilancia", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10682, 5, "51350597", "513505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aseo y vigilancia", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10683, 5, "513520", "5135", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Procesamiento electrónico de datos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10684, true, 5, "51352001", "513520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Servicios - Procesamiento electrónico de datos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10685, 5, "51352097", "513520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal procesamiento electrónico de datos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10686, 5, "513525", "5135", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Acueducto y alcantarillado", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10687, true, 5, "51352501", "513525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Servicios públicos - Acueducto y alcantarillado", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10688, 5, "51352597", "513525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal acueducto y alcantarillado", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10689, 5, "513530", "5135", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Energía eléctrica", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10690, true, 5, "51353001", "513530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Servicios públicos - Energía eléctrica", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10691, 5, "51353097", "513530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal energía eléctrica", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10692, 5, "513535", "5135", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Teléfono", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10693, true, 5, "51353501", "513535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Servicios públicos - Teléfono", true, false, false, false, "M", null },
                    { 10694, true, 5, "51353502", "513535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Servicios públicos - Celular", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10695, 5, "51353597", "513535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal servicio teléfono", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10696, 5, "513540", "5135", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Correo portes y telegramas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10697, 5, "51354001", "513540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Correo portes y telegramas", true, false, false, false, "M", null },
                    { 10698, 5, "51354097", "513540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal correo portes y telegramas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10699, 5, "513550", "5135", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Transporte fletes y acarreos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10700, 5, "51355001", "513550", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Transporte fletes y acarreos", true, false, false, false, "M", null },
                    { 10701, 5, "51355097", "513550", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal transporte fletes y acarreos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10702, 5, "513555", "5135", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Gas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10703, true, 5, "51355501", "513555", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Servicios públicos - Gas", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10704, 5, "51355597", "513555", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal gas", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10705, 5, "513595", "5135", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10706, 5, "51359501", "513595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otros", true, false, false, false, "M", null },
                    { 10707, 5, "51359597", "513595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal otros", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10708, 5, "5140", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Gastos legales", false, false, false, "M", null },
                    { 10709, 5, "514005", "5140", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Notariales", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10710, true, 5, "51400501", "514005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Notariales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10711, 5, "51400597", "514005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal notariales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10712, 5, "514010", "5140", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Registro mercantil", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10713, true, 5, "51401001", "514010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Registro mercantil", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10714, 5, "51401097", "514010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal registro mercantil", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10715, 5, "514015", "5140", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Tramites y licencias", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10716, 5, "51401501", "514015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Tramites y licencias", true, false, false, false, "M", null },
                    { 10717, 5, "51401597", "514015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal tramites y licencias", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10718, 5, "514095", "5140", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10719, 5, "51409501", "514095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otros", true, false, false, false, "M", null },
                    { 10720, 5, "51409597", "514095", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal otros", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10721, 5, "5145", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Mantenimiento y reparaciones", false, false, false, "M", null },
                    { 10722, 5, "514510", "5145", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Construcciones y edificaciones", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10723, true, 5, "51451001", "514510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Mantenimientos - Construcciones y edificaciones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10724, 5, "51451097", "514510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal construcciones y edificaciones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10725, 5, "514520", "5145", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Equipo de oficina", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10726, 5, "51452001", "514520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Equipo de oficina", true, false, false, false, "M", null },
                    { 10727, 5, "51452097", "514520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal equipo de oficina", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10728, 5, "514525", "5145", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Equipo de computación y comunicación", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10729, true, 5, "51452501", "514525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Mantenimiento - Equipo de computación y comunicación", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10730, 5, "51452597", "514525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal equipo de computación y comunicación", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10731, 5, "514540", "5145", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Flota y equipo de transporte", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10732, 5, "51454001", "514540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Flota y equipo de transporte", true, false, false, false, "M", null },
                    { 10733, 5, "51454097", "514540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal flota y equipo de transporte", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10734, 5, "5150", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Adecuación e instalación", false, false, false, "M", null },
                    { 10735, 5, "515005", "5150", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Instalaciones eléctricas", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10736, 5, "51500501", "515005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Instalaciones eléctricas", true, false, false, false, "M", null },
                    { 10737, 5, "51500597", "515005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal instalaciones eléctricas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10738, 5, "515010", "5150", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Arreglos ornamentales", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10739, 5, "51501001", "515010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Arreglos ornamentales", true, false, false, false, "M", null },
                    { 10740, 5, "51501097", "515010", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal arreglos ornamentales", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10741, 5, "515015", "5150", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Reparaciones locativas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10742, 5, "51501501", "515015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Reparaciones locativas", true, false, false, false, "M", null },
                    { 10743, 5, "51501597", "515015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal reparaciones locativas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10744, 5, "515020", "5150", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Adecuación de puestos de trabajo", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10745, 5, "51502001", "515020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Adecuación de puestos de trabajo", true, false, false, false, "M", null },
                    { 10746, 5, "51502097", "515020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal adecuación de puestos de trabajo", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10747, 5, "5155", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Gastos de viaje", false, false, false, "M", null },
                    { 10748, 5, "515505", "5155", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Alojamiento y manutención", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10749, true, 5, "51550501", "515505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos de viaje - Alojamiento y manutención", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10750, 5, "51550597", "515505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal alojamiento y manutención", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10751, 5, "515515", "5155", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Pasajes aéreos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10752, true, 5, "51551501", "515515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos de viaje - Pasajes aéreos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10753, 5, "51551597", "515515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal pasajes aéreos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10754, 5, "515520", "5155", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Pasajes terrestres", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10755, true, 5, "51552001", "515520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos de viaje - Pasajes terrestres", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10756, 5, "51552097", "515520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal pasajes terrestres", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10757, 5, "515595", "5155", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros gastos de viaje", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10758, 5, "51559501", "515595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otros gastos de viaje", true, false, false, false, "M", null },
                    { 10759, 5, "51559597", "515595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal otros gastos de viaje", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10760, 5, "5160", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Depreciaciones", false, false, false, "M", null },
                    { 10761, 5, "516005", "5160", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Construcciones y edificaciones", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10762, 5, "51600501", "516005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Construcciones y edificaciones", true, false, false, false, "M", null },
                    { 10763, 5, "51600597", "516005", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal construcciones y edificaciones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10764, 5, "516015", "5160", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Equipo de oficina", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10765, true, 5, "51601501", "516015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Equipo de oficina", true, false, false, false, "M", null },
                    { 10766, true, 5, "51601597", "516015", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal equipo de oficina", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10767, 5, "516020", "5160", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Equipo de computación y comunicación", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10768, true, 5, "51602001", "516020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Equipo de computación y comunicación", true, false, false, false, "M", null },
                    { 10769, true, 5, "51602097", "516020", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal equipo de computación y comunicación", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10770, 5, "516035", "5160", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Flota y equipo de transporte", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10771, true, 5, "51603501", "516035", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Flota y equipo de transporte", true, false, false, false, "M", null },
                    { 10772, true, 5, "51603597", "516035", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal flota y equipo de transporte", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10773, 5, "5165", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Amortizaciones", false, false, false, "M", null },
                    { 10774, 5, "516510", "5165", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Intangibles", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10775, 5, "51651001", "516510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Intangibles", true, false, false, false, "M", null },
                    { 10776, 5, "51651097", "516510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal intangibles", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10777, 5, "516515", "5165", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Cargos diferidos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10778, 5, "51651501", "516515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Cargos diferidos", true, false, false, false, "M", null },
                    { 10779, 5, "51651597", "516515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal cargos diferidos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10780, 5, "5195", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Diversos", false, false, false, "M", null },
                    { 10781, 5, "519510", "5195", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Libros suscripciones periódicos y revistas", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10782, 5, "51951001", "519510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Libros suscripciones periódicos y revistas", true, false, false, false, "M", null },
                    { 10783, 5, "51951097", "519510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal libros suscripciones periódicos y revistas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10784, 5, "519520", "5195", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Gastos de representación y relaciones publicas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10785, 5, "51952001", "519520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos de representación y relaciones publicas", true, false, false, false, "M", null },
                    { 10786, 5, "51952097", "519520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal gastos de representación y relaciones publicas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10787, 5, "519525", "5195", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Elementos de aseo y cafetería", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10788, true, 5, "51952501", "519525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Elementos de aseo y cafetería", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10789, 5, "51952597", "519525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal elementos de aseo y cafetería", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10790, 5, "519530", "5195", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Útiles papelería y fotocopias", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10791, true, 5, "51953001", "519530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Útiles papelería y fotocopias", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10792, 5, "51953097", "519530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal útiles papelería y fotocopias", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10793, 5, "519535", "5195", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Combustibles y lubricantes", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10794, true, 5, "51953501", "519535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Combustibles y lubricantes", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10795, 5, "51953597", "519535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal combustibles y lubricantes", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10796, 5, "519545", "5195", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Taxis y buses", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10797, true, 5, "51954501", "519545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Taxis y buses", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10798, 5, "51954597", "519545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal taxis y buses", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10799, 5, "519560", "5195", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Casino y restaurante", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10800, true, 5, "51956001", "519560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Casino y restaurante", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10801, 5, "51956097", "519560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal casino y restaurante", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10802, 5, "519565", "5195", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Parqueaderos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10803, true, 5, "51956501", "519565", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Parqueaderos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10804, 5, "51956597", "519565", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal parqueaderos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10805, 5, "519595", "5195", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10806, 5, "51959501", "519595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otros", true, false, false, false, "M", null },
                    { 10807, 5, "51959597", "519595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal Otros", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10808, 5, "5199", "51", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Otros gastos", false, false, false, "M", null },
                    { 10809, 5, "519905", "5199", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Inversiones", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10810, 5, "51990501", "519905", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Provisiones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10811, 5, "519999", "5199", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros gastos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10812, true, 5, "51999999", "519999", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Pregúntale a tu contador", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10813, 5, "52", "5", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Ventas", false, false, false, "M", null },
                    { 10814, 5, "5205", "52", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Gastos de personal", false, false, false, "M", null },
                    { 10815, 5, "520503", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Salario integral", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10816, true, 5, "52050301", "520503", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Salario integral", true, false, false, false, "M", null },
                    { 10817, true, 5, "52050397", "520503", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal salario integral", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10818, 5, "520506", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Sueldos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10819, true, 5, "52050601", "520506", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Sueldos", true, false, false, false, "M", null },
                    { 10820, true, 5, "52050697", "520506", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal sueldos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10821, 5, "520512", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Apoyo sostenimiento aprendices", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10822, true, 5, "52051201", "520512", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Apoyo sostenimiento aprendices", true, false, false, false, "M", null },
                    { 10823, true, 5, "52051297", "520512", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal apoyo sostenimiento aprendices", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10824, 5, "520515", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Horas extras y recargos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10825, true, 5, "52051501", "520515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Horas extras y recargos", true, false, false, false, "M", null },
                    { 10826, true, 5, "52051597", "520515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal horas extras y recargos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10827, 5, "520524", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Incapacidades", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10828, true, 5, "52052401", "520524", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Incapacidades", true, false, false, false, "M", null },
                    { 10829, true, 5, "52052497", "520524", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal incapacidades", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10830, 5, "520527", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Auxilio de transporte", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10831, true, 5, "52052701", "520527", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Auxilio de transporte", true, false, false, false, "M", null },
                    { 10832, true, 5, "52052797", "520527", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal auxilio de transporte", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10833, 5, "520530", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Cesantías", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10834, true, 5, "52053001", "520530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Cesantías", true, false, false, false, "M", null },
                    { 10835, true, 5, "52053097", "520530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal cesantías", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10836, 5, "520533", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Intereses sobre cesantías", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10837, true, 5, "52053301", "520533", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Intereses sobre cesantías", true, false, false, false, "M", null },
                    { 10838, true, 5, "52053397", "520533", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal intereses sobre cesantías", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10839, 5, "520536", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Prima de servicios", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10840, true, 5, "52053601", "520536", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Prima de servicios", true, false, false, false, "M", null },
                    { 10841, true, 5, "52053697", "520536", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal prima de servicios", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10842, 5, "520539", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Vacaciones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10843, true, 5, "52053901", "520539", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Vacaciones", true, false, false, false, "M", null },
                    { 10844, true, 5, "52053997", "520539", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal vacaciones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10845, 5, "520545", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Auxilios", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10846, true, 5, "52054501", "520545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Educativo", true, false, false, false, "M", null },
                    { 10847, true, 5, "52054502", "520545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Auxilio transporte", true, false, false, false, "M", null },
                    { 10848, true, 5, "52054597", "520545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal auxilio transporte", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10849, 5, "520548", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Bonificaciones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10850, true, 5, "52054801", "520548", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Bonificaciones", true, false, false, false, "M", null },
                    { 10851, true, 5, "52054897", "520548", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal bonificaciones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10852, 5, "520551", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Dotación y suministro a trabajadores", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10853, true, 5, "52055101", "520551", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Dotación y suministro a trabajadores", true, false, false, false, "M", null },
                    { 10854, true, 5, "52055197", "520551", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal dotación y suministro a trabajadores", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10855, 5, "520560", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Indemnizaciones laborales", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10856, true, 5, "52056001", "520560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Indemnizaciones laborales", true, false, false, false, "M", null },
                    { 10857, true, 5, "52056097", "520560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal indemnizaciones laborales", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10858, 5, "520563", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Capacitación al personal", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10859, true, 5, "52056301", "520563", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Capacitación al personal", true, false, false, false, "M", null },
                    { 10860, true, 5, "52056397", "520563", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal capacitación al personal", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10861, 5, "520566", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Gastos deportivos y de recreación", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10862, true, 5, "52056601", "520566", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos deportivos y de recreación", true, false, false, false, "M", null },
                    { 10863, true, 5, "52056697", "520566", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal gastos deportivos y de recreación", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10864, 5, "520568", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes a administradora de riesgos laborales", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10865, true, 5, "52056801", "520568", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes a administradora de riesgos laborales", true, false, false, false, "M", null },
                    { 10866, true, 5, "52056897", "520568", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes a administradora de riesgos laborales", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10867, 5, "520569", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes a entidades promotoras de salud eps", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10868, true, 5, "52056901", "520569", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes a entidades promotoras de salud eps", true, false, false, false, "M", null },
                    { 10869, true, 5, "52056997", "520569", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes a entidades promotoras de salud eps", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10870, 5, "520570", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aporte a fondos de pensión y/o cesantías", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10871, true, 5, "52057001", "520570", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aporte a fondos de pensión y/o cesantías", true, false, false, false, "M", null },
                    { 10872, true, 5, "52057097", "520570", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aporte a fondos de pensión y/o cesantías", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10873, 5, "520572", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes cajas de compensación familiar", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10874, true, 5, "52057201", "520572", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes cajas de compensación familiar", true, false, false, false, "M", null },
                    { 10875, true, 5, "52057297", "520572", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes cajas de compensación familiar", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10876, 5, "520575", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes icbf", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10877, true, 5, "52057501", "520575", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes icbf", true, false, false, false, "M", null },
                    { 10878, true, 5, "52057597", "520575", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes icbf", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10879, 5, "520578", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes Sena", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10880, true, 5, "52057801", "520578", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes Sena", true, false, false, false, "M", null },
                    { 10881, true, 5, "52057897", "520578", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes Sena", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10882, 5, "520584", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Gastos médicos y drogas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10883, true, 5, "52058401", "520584", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos médicos y drogas", true, false, false, false, "M", null },
                    { 10884, true, 5, "52058497", "520584", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal gastos médicos y drogas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10885, 5, "520595", "5205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10886, true, 5, "52059501", "520595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Bienestar y atención a empleados", true, false, false, false, "M", null },
                    { 10887, true, 5, "52059597", "520595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal bienestar y atención a empleados", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10888, 5, "5235", "52", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Servicios", false, false, false, "M", null },
                    { 10889, 5, "523510", "5235", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Temporales", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10890, 5, "52351001", "523510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Temporales", true, false, false, false, "M", null },
                    { 10891, 5, "52351097", "523510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal temporales", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10892, 5, "523535", "5235", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Teléfono", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10893, 5, "52353501", "523535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Teléfono", true, false, false, false, "M", null },
                    { 10894, 5, "52353597", "523535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal teléfono", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10895, 5, "523540", "5235", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Correo portes y telegramas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10896, 5, "52354001", "523540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Correo portes y telegramas", true, false, false, false, "M", null },
                    { 10897, 5, "52354097", "523540", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal correo portes y telegramas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10898, 5, "523560", "5235", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Publicidad propaganda y promoción", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10899, 5, "52356001", "523560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Medios", true, false, false, false, "M", null },
                    { 10900, 5, "52356002", "523560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Directorio telefónico", true, false, false, false, "M", null },
                    { 10901, 5, "52356003", "523560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Material pop", true, false, false, false, "M", null },
                    { 10902, 5, "52356004", "523560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Marketing corporativo", true, false, false, false, "M", null },
                    { 10903, 5, "52356005", "523560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Premios y eventos", true, false, false, false, "M", null },
                    { 10904, 5, "52356006", "523560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "IVA asumido en obsequios y muestras gratis", true, false, false, false, "M", null },
                    { 10905, 5, "52356097", "523560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal premios y eventos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10906, 5, "5255", "52", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Gastos de viaje", false, false, false, "M", null },
                    { 10907, 5, "525515", "5255", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Pasajes aéreos comercial", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10908, 5, "52551501", "525515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Pasajes aéreos comercial", true, false, false, false, "M", null },
                    { 10909, 5, "52551597", "525515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal pasajes aéreos comercial", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10910, 5, "5295", "52", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Diversos", false, false, false, "M", null },
                    { 10911, 5, "529505", "5295", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Comisiones", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10912, true, 5, "52950501", "529505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Comisiones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10913, 5, "52950597", "529505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal comisiones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10914, 5, "529545", "5295", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Taxis y buses", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10915, 5, "52954501", "529545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Taxis y buses", true, false, false, false, "M", null },
                    { 10916, 5, "52954597", "529545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal taxis y buses", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10917, 5, "529595", "5295", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros diversos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10918, 5, "52959501", "529595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Atención al cliente", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10919, true, 5, "52959505", "529595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos diversos POS", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10920, 5, "52959597", "529595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal atención al cliente", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10921, 5, "53", "5", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Otros gastos de actividades ordinarias", false, false, false, "M", null },
                    { 10922, 5, "5305", "53", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Financieros", false, false, false, "M", null },
                    { 10923, 5, "530505", "5305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Gastos bancarios", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10924, 5, "53050501", "530505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos bancarios", true, false, false, false, "M", null },
                    { 10925, 5, "53050597", "530505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal gastos bancarios", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10926, 5, "530515", "5305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Comisiones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10927, 5, "53051501", "530515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Comisiones", true, false, false, false, "M", null },
                    { 10928, 5, "53051597", "530515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal comisiones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10929, 5, "530520", "5305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Intereses", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10930, 5, "53052001", "530520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Intereses corrientes", true, false, false, false, "M", null },
                    { 10931, 5, "53052002", "530520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Intereses de mora", true, false, false, false, "M", null },
                    { 10932, 5, "53052097", "530520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal intereses de mora", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10933, 5, "530525", "5305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Diferencia en cambio", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10934, 5, "53052501", "530525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Diferencia en cambio", true, false, false, false, "M", null },
                    { 10935, 5, "53052597", "530525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal diferencia en cambio", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10936, 5, "530535", "5305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Descuentos comerciales condicionados", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10937, true, 5, "53053501", "530535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Descuentos comerciales condicionados", true, false, false, false, "M", null },
                    { 10938, true, 5, "53053502", "530535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Descuentos comerciales condicionados POS", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10939, 5, "53053597", "530535", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal descuentos comerciales condicionados", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10940, 5, "5310", "53", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Perdida en venta y retiro de bienes", false, false, false, "M", null },
                    { 10941, 5, "531030", "5310", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Retiro de propiedades planta y equipo", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10942, 5, "53103001", "531030", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Retiro de propiedades planta y equipo", true, false, false, false, "M", null },
                    { 10943, 5, "53103097", "531030", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal retiro de propiedades planta y equipo", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10944, 5, "5315", "53", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Gastos extraordinarios", false, false, false, "M", null },
                    { 10945, 5, "531515", "5315", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Costos y gastos de ejercicios anteriores", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10946, 5, "53151501", "531515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Costos y gastos de ejercicios anteriores", true, false, false, false, "M", null },
                    { 10947, 5, "53151597", "531515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal costos y gastos de ejercicios anteriores", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10948, 5, "531520", "5315", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Impuestos asumidos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10949, 5, "53152001", "531520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gravamen al movimiento financiero", true, false, false, false, "M", null },
                    { 10950, 5, "53152002", "531520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otros impuestos asumidos", true, false, false, false, "M", null },
                    { 10951, 5, "53152097", "531520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal impuestos asumidos", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10952, 5, "531525", "5315", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Costos y gastos no deducibles", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10953, 5, "53152501", "531525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Costos y gastos no deducibles", true, false, false, false, "M", null },
                    { 10954, 5, "53152597", "531525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal costos y gastos no deducibles", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10955, 5, "5395", "53", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Gastos diversos", false, false, false, "M", null },
                    { 10956, 5, "539520", "5395", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Multas sanciones y litigios", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10957, 5, "53952001", "539520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Multas sanciones y litigios", true, false, false, false, "M", null },
                    { 10958, 5, "53952097", "539520", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal multas sanciones y litigios", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10959, 5, "539525", "5395", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Donaciones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10960, 5, "53952501", "539525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Donaciones", true, false, false, false, "M", null },
                    { 10961, 5, "53952597", "539525", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal donaciones", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10962, 5, "539581", "5395", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Ajuste al peso", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10963, 5, "53958101", "539581", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Ajuste al peso", true, false, false, false, "M", null },
                    { 10964, 5, "53958197", "539581", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal ajuste al peso", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10965, 5, "539595", "5395", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10966, 5, "53959501", "539595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Otros", true, false, false, false, "M", null },
                    { 10967, 5, "53959597", "539595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal otros", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10968, 5, "54", "5", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Impuesto de renta y complementarios", false, false, false, "M", null },
                    { 10969, 5, "5405", "54", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Impuesto de renta y complementarios", false, false, false, "M", null },
                    { 10970, 5, "540505", "5405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Impuesto de renta y complementarios", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10971, 5, "54050501", "540505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto de renta y complementarios", true, false, false, false, "M", null },
                    { 10972, 5, "54050597", "540505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal impuesto de renta y complementarios", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10973, 5, "540510", "5405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Cree", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10974, 5, "54051001", "540510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Cree", true, false, false, false, "M", null },
                    { 10975, 5, "54051097", "540510", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal cree", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10976, 5, "540515", "5405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Impuesto a la riqueza", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10977, 5, "54051501", "540515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Impuesto a la riqueza", true, false, false, false, "M", null },
                    { 10978, 5, "54051597", "540515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal impuesto a la riqueza", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10979, 6, "6", null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 1, "Costos de venta", false, false, false, "M", null },
                    { 10980, 6, "61", "6", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Costo de ventas y de prestación de servicios", false, false, false, "M", null },
                    { 10981, 6, "6135", "61", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Comercio al por mayor y al por menor", false, false, false, "M", null },
                    { 10982, 6, "613505", "6135", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Comercio al por mayor y al por menor", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10983, true, 6, "61350501", "613505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Comercio al por mayor y al por menor", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10984, 6, "61350597", "613505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal comercio al por mayor y al por menor", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10985, 6, "6180", "61", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Servicios", false, false, false, "M", null },
                    { 10986, 6, "618001", "6180", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Servicios", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10987, true, 6, "61800101", "618001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Servicios", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10988, 6, "61800197", "618001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal servicios", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10989, 7, "7", null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 1, "Costos de producción", false, false, false, "M", null },
                    { 10990, 7, "71", "7", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Costos de producción o de operación", false, false, false, "M", null },
                    { 10991, 7, "7105", "71", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Costos de producción o de operación", false, false, false, "M", null },
                    { 10992, 7, "710505", "7105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Costos de producción o de operación", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10993, true, 7, "71050501", "710505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Materia prima", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10994, 7, "71050597", "710505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal materia prima", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 10995, 7, "72", "7", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Mano de obra directa", false, false, false, "M", null },
                    { 10996, 7, "7205", "72", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Mano de obra directa", false, false, false, "M", null },
                    { 10997, 7, "720503", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Salario integral", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10998, true, 7, "72050301", "720503", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Salario integral", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 10999, 7, "72050397", "720503", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal salario integral", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11000, 7, "720506", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Sueldos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11001, true, 7, "72050601", "720506", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Sueldos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11002, 7, "72050697", "720506", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal sueldos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11003, 7, "720512", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Apoyo sostenimiento aprendices", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11004, true, 7, "72051201", "720512", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Apoyo sostenimiento aprendices", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11005, 7, "72051297", "720512", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal apoyo sostenimiento aprendices", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11006, 7, "720515", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Horas extras y recargos", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11007, true, 7, "72051501", "720515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Horas extras y recargos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11008, 7, "72051597", "720515", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal horas extras y recargos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11009, 7, "720524", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Incapacidades", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11010, true, 7, "72052401", "720524", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Incapacidades", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11011, 7, "72052497", "720524", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal incapacidades", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11012, 7, "720527", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Auxilio de transporte", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11013, true, 7, "72052701", "720527", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Auxilio de transporte", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11014, 7, "72052797", "720527", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal auxilio de transporte", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11015, 7, "720530", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Cesantías", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11016, true, 7, "72053001", "720530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Cesantías", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11017, 7, "72053097", "720530", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal cesantías", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11018, 7, "720533", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Intereses sobre cesantías", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11019, true, 7, "72053301", "720533", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Intereses sobre cesantías", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11020, 7, "72053397", "720533", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal intereses sobre cesantías", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11021, 7, "720536", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Prima de servicios", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11022, true, 7, "72053601", "720536", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Prima de servicios", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11023, 7, "72053697", "720536", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal prima de servicios", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11024, 7, "720539", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Vacaciones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11025, true, 7, "72053901", "720539", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Vacaciones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11026, 7, "72053997", "720539", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal vacaciones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11027, 7, "720545", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Auxilios", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 11028, 7, "72054501", "720545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Educativo", true, false, false, false, "M", null },
                    { 11029, 7, "72054502", "720545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Auxilio transporte", true, false, false, false, "M", null },
                    { 11030, 7, "72054597", "720545", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal auxilio transporte", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11031, 7, "720548", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Bonificaciones", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11032, true, 7, "72054801", "720548", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Bonificaciones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11033, 7, "72054897", "720548", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal bonificaciones", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11034, 7, "720551", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Dotación y suministro a trabajadores", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 11035, 7, "72055101", "720551", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Dotación y suministro a trabajadores", true, false, false, false, "M", null },
                    { 11036, 7, "72055197", "720551", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal dotación y suministro a trabajadores", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11037, 7, "720560", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Indemnizaciones laborales", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11038, true, 7, "72056001", "720560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Indemnizaciones laborales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11039, 7, "72056097", "720560", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal indemnizaciones laborales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11040, 7, "720563", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Capacitación al personal", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 11041, 7, "72056301", "720563", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Capacitación al personal", true, false, false, false, "M", null },
                    { 11042, 7, "72056397", "720563", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal capacitación al personal", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11043, 7, "720566", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Gastos deportivos y de recreación", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 11044, 7, "72056601", "720566", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos deportivos y de recreación", true, false, false, false, "M", null },
                    { 11045, 7, "72056697", "720566", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal gastos deportivos y de recreación", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11046, 7, "720568", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes a administradora de riesgos laborales", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11047, true, 7, "72056801", "720568", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes a administradora de riesgos laborales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11048, 7, "72056897", "720568", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes a administradora de riesgos laborales", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11049, 7, "720569", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes a entidades promotoras de salud eps", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11050, true, 7, "72056901", "720569", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes a entidades promotoras de salud eps", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11051, 7, "72056997", "720569", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes a entidades promotoras de salud eps", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11052, 7, "720570", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aporte a fondos de pensión y/o cesantías", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11053, true, 7, "72057001", "720570", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aporte a fondos de pensión y/o cesantías", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11054, 7, "72057097", "720570", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aporte a fondos de pensión y/o cesantías", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11055, 7, "720572", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes cajas de compensación familiar", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11056, true, 7, "72057201", "720572", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes cajas de compensación familiar", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11057, 7, "72057297", "720572", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes cajas de compensación familiar", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11058, 7, "720575", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes icbf", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11059, true, 7, "72057501", "720575", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes icbf", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11060, 7, "72057597", "720575", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes icbf", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11061, 7, "720578", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Aportes Sena", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11062, true, 7, "72057801", "720578", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Aportes Sena", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11063, 7, "72057897", "720578", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal aportes Sena", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11064, 7, "720584", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Gastos médicos y drogas", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 11065, 7, "72058401", "720584", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Gastos médicos y drogas", true, false, false, false, "M", null },
                    { 11066, 7, "72058497", "720584", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal gastos médicos y drogas", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11067, 7, "720595", "7205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Otros", false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 11068, 7, "72059501", "720595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Bienestar y atención a empleados", true, false, false, false, "M", null },
                    { 11069, 7, "72059597", "720595", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal bienestar y atención a empleados", true, false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 11070, 7, "73", "7", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Costos indirectos", false, false, false, "M", null },
                    { 11071, 7, "7305", "73", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Costos indirectos", false, false, false, "M", null },
                    { 11072, 7, "730505", "7305", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Costos indirectos", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11073, true, 7, "73050501", "730505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Costos indirectos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11074, 7, "73050597", "730505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal costos indirectos", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 11075, 7, "74", "7", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Contratos de servicios", false, false, false, "M", null },
                    { 11076, 7, "7405", "74", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 3, "Contratos de servicios", false, false, false, "M", null },
                    { 11077, 7, "740505", "7405", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 4, "Contratos de servicios", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11078, true, 7, "74050501", "740505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "Contratos de servicios", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11079, 7, "74050597", "740505", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 5, "D. fiscal contratos de servicios", true, false, false, false, "M", null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[,]
                {
                    { 11080, 8, "8", null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 1, "Cuentas de orden deudoras", false, false, false, "M", null },
                    { 11081, 8, "81", "8", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", 2, "Derechos contingentes", false, false, false, "M", null },
                    { 11082, 9, "9", null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 1, "Cuentas de orden acreedoras", false, false, false, "M", null },
                    { 11083, 9, "99", "9", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 2, "Cuentas de orden acreedoras", false, false, false, "M", null },
                    { 11084, 9, "9999", "99", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 3, "Saldos iniciales por conciliar", false, false, false, "M", null },
                    { 11085, 9, "999999", "9999", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 4, "Saldos iniciales por conciliar", false, false, false, "M", null }
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Activa", "ClasePUC", "Codigo", "CodigoPadre", "Descripcion", "FechaCreacion", "Naturaleza", "Nivel", "Nombre", "PermiteMovimiento", "RequiereCentroCosto", "RequiereDocumento", "RequiereTercero", "TipoAjuste", "UsuarioId" },
                values: new object[] { 11086, true, 9, "99999999", "999999", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", 5, "Saldos iniciales por conciliar", true, false, false, false, "M", null });

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
                    { 1, null, 26, 10290, 10087, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Autorretención 0.40%", 0.40m, "Autoretención 2201", "Pesos", null },
                    { 2, null, 27, 10290, 10087, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Autorretención 0.80%", 0.80m, "Autoretención 2201", "Pesos", null },
                    { 3, null, 28, 10290, 10087, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Autorretención 1.60%", 1.60m, "Autoretención 2201", "Pesos", null }
                });

            migrationBuilder.InsertData(
                table: "Cupones",
                columns: new[] { "Id", "Codigo", "DescuentoPorcentaje", "Expiracion", "IsActive", "MaxUsos", "PlanId", "UsosCodigo" },
                values: new object[,]
                {
                    { 2, "NUBEE S.A.SPRO", 30m, null, true, 30, 3, 0 },
                    { 3, "STARTEFC25", 12m, null, true, 20, 1, 0 }
                });

            migrationBuilder.InsertData(
                table: "Impuestos",
                columns: new[] { "Id", "Codigo", "CodigoTributoDIAN", "CuentaContableId", "CuentaContableId1", "CuentaCreditoComprasId", "CuentaCreditoVentasId", "CuentaDebitoComprasId", "CuentaDebitoVentasId", "CuentaDevolucionComprasId", "CuentaDevolucionVentasId", "EnUso", "FechaCreacion", "Nombre", "PorValor", "Tarifa", "TipoImpuesto", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 1, "01", null, null, null, 10362, 10366, null, 10367, 10375, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IVA 19%", false, 19.00m, "IVA", null },
                    { 2, 2, "01", null, null, null, 10363, 10368, null, 10369, 10376, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IVA 5%", false, 5.00m, "IVA", null },
                    { 3, 3, "05", null, null, 10249, null, null, 10077, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 11%", false, 11.00m, "Retefuente", null },
                    { 4, 4, "05", null, null, 10281, null, null, 10075, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 10%", false, 10.00m, "Retefuente", null },
                    { 5, 5, "05", null, null, 10265, null, null, 10073, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 6%", false, 6.00m, "Retefuente", null },
                    { 6, 6, "05", null, null, 10267, null, null, 10071, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 4%", false, 4.00m, "Retefuente", null },
                    { 7, 7, "05", null, null, 10281, null, null, 10069, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 2.5%", false, 2.50m, "Retefuente", null },
                    { 8, 8, "06", null, null, 10305, null, null, 10095, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 11.04", false, 11.04m, "ReteICA", null },
                    { 9, 9, "06", null, null, 10307, null, null, 10097, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 13.8", false, 13.80m, "ReteICA", null },
                    { 10, 10, "06", null, null, 10309, null, null, 10099, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 9.66", false, 9.66m, "ReteICA", null },
                    { 11, 11, "06", null, null, 10311, null, null, 10101, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 8", false, 8.00m, "ReteICA", null },
                    { 12, 12, "06", null, null, 10313, null, null, 10103, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 7", false, 7.00m, "ReteICA", null },
                    { 13, 13, "06", null, null, 10315, null, null, 10105, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 6.9", false, 6.90m, "ReteICA", null },
                    { 14, 14, "06", null, null, 10317, null, null, 10107, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteICA 4.14", false, 4.14m, "ReteICA", null },
                    { 15, 15, "04", null, null, 10294, null, null, 10089, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteIVA 15%", false, 15.00m, "ReteIVA", null },
                    { 16, 16, "02", null, null, null, 10401, 10403, null, 10404, 10402, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Impoconsumo 8%", false, 8.00m, "Impoconsumo", null },
                    { 17, 17, "02", null, null, null, 10401, 10403, null, 10404, 10402, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Impoconsumo por valor", false, 0.00m, "Impoconsumo", null },
                    { 18, 18, "05", null, null, 10253, null, null, 10081, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 3.5%", false, 3.50m, "Retefuente", null },
                    { 19, 19, "05", null, null, 10251, null, null, 10079, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 7%", false, 7.00m, "Retefuente", null },
                    { 20, 20, "05", null, null, 10255, null, null, 10083, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 2%", false, 2.00m, "Retefuente", null },
                    { 21, 21, "05", null, null, 10257, null, null, 10085, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 1%", false, 1.00m, "Retefuente", null },
                    { 22, 22, "01", null, null, null, 10362, 10366, null, 10367, 10375, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IVA 0%", false, 0.00m, "IVA", null },
                    { 23, 23, "01", null, null, null, 10364, 10370, null, 10371, 10377, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IVA 16%", false, 16.00m, "IVA", null },
                    { 24, 24, "ZY", null, null, null, 10386, 10632, null, 10634, 10389, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AdValorem 20%", false, 20.00m, "Ad-Valorem", null },
                    { 25, 25, "ZY", null, null, null, 10387, 10633, null, 10635, 10390, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AdValorem 25%", false, 25.00m, "Ad-Valorem", null },
                    { 29, 29, "04", null, null, 10296, null, null, 10091, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ReteIVA 100%", false, 100.00m, "ReteIVA", null },
                    { 90, 90, "05", null, null, 10281, null, null, 10069, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 1.5%", false, 1.50m, "Retefuente", null },
                    { 91, 91, "05", null, null, 10281, null, null, 10069, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 0.10%", false, 0.10m, "Retefuente", null },
                    { 92, 92, "05", null, null, 10281, null, null, 10069, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 0.50%", false, 0.50m, "Retefuente", null },
                    { 93, 93, "05", null, null, 10281, null, null, 10069, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Retefuente 20%", false, 20.00m, "Retefuente", null },
                    { 94, 94, "ZA", null, null, null, 10391, 10393, null, 10394, 10392, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comestibles ultraprocesados 15%", false, 15.00m, "Comestibles ultraprocesados", null },
                    { 95, 95, "ZA", null, null, null, 10395, 10397, null, 10398, 10396, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comestibles ultraprocesados 20%", false, 20.00m, "Comestibles ultraprocesados", null }
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
                name: "IX_ConfiguracionesImpuestoEmpresa_TarifaImpuestoId",
                table: "ConfiguracionesImpuestoEmpresa",
                column: "TarifaImpuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionImpuesto_Empresa_Tarifa",
                table: "ConfiguracionesImpuestoEmpresa",
                columns: new[] { "EmpresaId", "TarifaImpuestoId" },
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
                name: "IX_DetalleFacturaImpuestos_DetalleFacturaId",
                table: "DetalleFacturaImpuestos",
                column: "DetalleFacturaId");

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
                name: "IX_DocumentoLineaImpuesto_Detalle_Tarifa",
                table: "DocumentosLineasImpuesto",
                columns: new[] { "DetalleFacturaId", "TarifaImpuestoId" });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosLineasImpuesto_TarifaImpuestoId",
                table: "DocumentosLineasImpuesto",
                column: "TarifaImpuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoResumenImpuesto_Factura_Tarifa",
                table: "DocumentosResumenImpuesto",
                columns: new[] { "FacturaId", "TarifaImpuestoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosResumenImpuesto_TarifaImpuestoId",
                table: "DocumentosResumenImpuesto",
                column: "TarifaImpuestoId");

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
                name: "IX_Impuestos_CuentaContableId",
                table: "Impuestos",
                column: "CuentaContableId");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_CuentaContableId1",
                table: "Impuestos",
                column: "CuentaContableId1");

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
                name: "IX_ImpuestoConcepto_Empresa_Codigo",
                table: "ImpuestosConceptos",
                columns: new[] { "EmpresaId", "CodigoInterno" },
                unique: true,
                filter: "[EmpresaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MapeoContable_Tarifa_Contexto_Rol",
                table: "MapeosContablesTarifa",
                columns: new[] { "TarifaImpuestoId", "Contexto", "RolCuenta" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapeosContablesTarifa_CuentaContableId",
                table: "MapeosContablesTarifa",
                column: "CuentaContableId");

            migrationBuilder.CreateIndex(
                name: "IX_Negocios_Nit",
                table: "Negocios",
                column: "Nit",
                unique: true,
                filter: "[Nit] IS NOT NULL");

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
                name: "IX_ReglaImpuesto_Tarifa_Activa",
                table: "ReglasImpuesto",
                columns: new[] { "TarifaImpuestoId", "Activa" });

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
                name: "IX_TarifaImpuesto_Concepto_Nombre",
                table: "TarifasImpuestos",
                columns: new[] { "ImpuestoConceptoId", "Nombre" },
                unique: true);

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
                name: "ConfiguracionesImpuestoEmpresa");

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
                name: "DocumentosLineasImpuesto");

            migrationBuilder.DropTable(
                name: "DocumentosResumenImpuesto");

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
                name: "MapeosContablesTarifa");

            migrationBuilder.DropTable(
                name: "PerfilesTributarios");

            migrationBuilder.DropTable(
                name: "PlanFeature");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RegistrosPendientes");

            migrationBuilder.DropTable(
                name: "ReglasImpuesto");

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
                name: "Impuestos");

            migrationBuilder.DropTable(
                name: "DetalleFacturas");

            migrationBuilder.DropTable(
                name: "NotasCredito");

            migrationBuilder.DropTable(
                name: "NotasDebito");

            migrationBuilder.DropTable(
                name: "TarifasImpuestos");

            migrationBuilder.DropTable(
                name: "Negocios");

            migrationBuilder.DropTable(
                name: "Addons");

            migrationBuilder.DropTable(
                name: "PlanesFacturacion");

            migrationBuilder.DropTable(
                name: "CuentasContables");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "ImpuestosConceptos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
