using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NubeeAPI.Data;
using NubeeAPI.Services;
using NubeeAPI.Services.AuthLogin;
using NubeeAPI.Services.Clientes;
using NubeeAPI.Services.Facturas;
using NubeeAPI.Services.Factus;
using NubeeAPI.Services.Notificaciones;
using NubeeAPI.Services.Productos;
using NubeeAPI.Services.Seguridad;
using NubeeAPI.Services.Usuarios;
using NubeeAPI.Services.Wompi;
using NubeeAPI.Utils.Exceptions;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Serilog;
using Serilog.Events;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// ===== Serilog: logging estructurado (consola + archivo rotativo diario) =====
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/factcloud-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 14,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Reemplaza el logging por defecto por Serilog
builder.Host.UseSerilog();

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>    {
        policy.SetIsOriginAllowed(origin =>
        origin.StartsWith("http://localhost") ||
        origin.StartsWith("https://localhost"))
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
        
    });
});

// ===== Controllers y JSON =====
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// ===== FluentValidation =====
builder.Services.AddScoped<
    FluentValidation.IValidator<NubeeAPI.Models.Factura>,
    NubeeAPI.Validators.FacturaFactusValidator>();

// ===== Opciones de Factus (Options pattern + validación al arrancar) =====
builder.Services.AddOptions<NubeeAPI.Configuration.FactusOptions>()
    .Bind(builder.Configuration.GetSection(NubeeAPI.Configuration.FactusOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// ===== HttpClient para Factus =====
// Resiliencia con Polly: timeout por intento + circuit breaker.
// ⚠️ NO se agrega RETRY aquí a propósito: FactusService ya reintenta de forma
//    "method-aware" (sólo GET; el POST /v2/bills/validate NUNCA se reintenta para
//    no emitir una factura duplicada ante la DIAN). Un retry a nivel handler
//    reintentaría también el POST y causaría doble emisión.
builder.Services.AddHttpClient("Factus", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Factus:BaseUrl"] ?? "https://api-sandbox.factus.com.co");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
    // Sin tope agresivo: el timeout por intento lo gobierna Polly más abajo.
    client.Timeout = TimeSpan.FromSeconds(100);
})
.AddResilienceHandler("factus-pipeline", pipeline =>
{
    // Timeout por intento individual
    pipeline.AddTimeout(TimeSpan.FromSeconds(30));

    // Circuit breaker: si Factus falla de forma sostenida, abrir el circuito
    // y dejar de golpear durante un tiempo (protege a Factus y libera hilos).
    pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
    {
        FailureRatio = 0.5,                              // 50% de fallos…
        SamplingDuration = TimeSpan.FromSeconds(30),     // …en una ventana de 30s
        MinimumThroughput = 5,                           // con al menos 5 llamadas
        BreakDuration = TimeSpan.FromSeconds(15)         // abre el circuito 15s
    });
});

// ===== Swagger =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa 'Bearer' [espacio] y tu token.\n\nEjemplo: 'Bearer eyJhbGciOiJIUzI1Ni...'"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
{
{
new Microsoft.OpenApi.Models.OpenApiSecurityScheme
{
Reference = new Microsoft.OpenApi.Models.OpenApiReference
{
Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
Id = "Bearer"
}
},
new string[] {}
}
});
});
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddScoped<SeguridadService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IFacturaService, FacturaService>();
builder.Services.AddScoped<IPosFacturacionService, PosFacturacionService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddSingleton<IFactusService, FactusService>();
builder.Services.AddHttpClient<IWompiService, WompiService>(client =>
{
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddScoped<IDocumentoSoporteService, DocumentoSoporteService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();

// ===== Base de datos =====
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(conn, sqlServerOptions =>
{
    // ✅ Cambiar a 0 reintentos
    sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 0);
}));

// ===== Servicios =====
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISuscripcionService, SuscripcionService>();
builder.Services.AddSingleton<NubeeAPI.Services.ConfiguracionService>();

// ===== JWT Config (robusto para diseño) =====
// Obtenemos configuraciones pero no lanzamos excepción en diseño
var jwtKeyConfig = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

var isDevelopment = builder.Environment.IsDevelopment();

// En Development permitimos una clave temporal para poder ejecutar dotnet ef sin variables de entorno
if (string.IsNullOrWhiteSpace(jwtKeyConfig) && isDevelopment)
{
    jwtKeyConfig = "dev-temporary-key-for-design-time-please-change";
}

// En producción exigimos la clave
if (string.IsNullOrWhiteSpace(jwtKeyConfig) && !isDevelopment)
{
    throw new InvalidOperationException("JWT Key missing");
}

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKeyConfig));

// Registrar autenticación normalmente; si signingKey es temporal seguirá funcionando en Dev
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    // Si no hay clave real, evitamos validaciones estrictas (solo en diseño)
    if (string.IsNullOrWhiteSpace(builder.Configuration["Jwt:Key"]) && isDevelopment)
    {
        options.RequireHttpsMetadata = false;
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context => Task.CompletedTask
        };
        return;
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = signingKey
    };

    // SignalR (WebSockets) no puede enviar el header Authorization desde el
    // navegador: el token llega por query string (?access_token=...). Lo leemos
    // sólo para la ruta del hub de notificaciones para que Clients.User() pueda
    // resolver el destinatario.
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/api/notificacionesHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// ===== Logging de peticiones HTTP (Serilog) =====
app.UseSerilogRequestLogging();

// ===== Manejo global de excepciones =====
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        context.Response.ContentType = "application/json";

        if (exception is BusinessException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                mensaje = exception.Message
            });
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                mensaje = "Error interno del servidor",
                errorReal = exception?.Message,
                detalle = exception?.InnerException?.Message
            });
        }
    });
});

// ===== Middleware ordenado =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/api/db-test", async (ApplicationDbContext db) =>
{
    try
    {
        await db.Database.CanConnectAsync();
        return Results.Ok("BD CONECTADA");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.UseCors("AllowReact");
app.UseHttpsRedirection();
app.UseAuthentication(); // 3. Autenticación (lee el token)
app.UseAuthorization(); // 4. Autorización (verifica permisos)
app.MapHub<NotificacionesHub>("/api/notificacionesHub").AllowAnonymous();

// ===== Auto-creación de tablas y migraciones incrementales =====
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();

        // Columna Rol en Usuarios (EnsureCreated no aplica cambios a tablas existentes)
        context.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (
                SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = 'Usuarios' AND COLUMN_NAME = 'Rol'
            )
            BEGIN
                ALTER TABLE Usuarios
                    ADD Rol NVARCHAR(30) NOT NULL
                    CONSTRAINT DF_Usuarios_Rol DEFAULT 'usuario';
            END");

        // Columna NumeroFactus en Facturas (número oficial devuelto por Factus)
        context.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (
                SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = 'Facturas' AND COLUMN_NAME = 'NumeroFactus'
            )
            BEGIN
                ALTER TABLE Facturas ADD NumeroFactus NVARCHAR(40) NULL;
            END");

        // Corrige el código DIAN del Impoconsumo (INC) en datos ya sembrados:
        // estándar DIAN/Factus/CUFE = 04 (antes quedó como 02). Idempotente.
        context.Database.ExecuteSqlRaw(@"
            UPDATE Impuestos
               SET CodigoTributoDIAN = '04'
             WHERE TipoImpuesto = 'Impoconsumo'
               AND CodigoTributoDIAN = '02';");

        // Tabla de desglose de formas de pago de factura (payment_details Factus)
        context.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FacturasFormasPago')
            BEGIN
                CREATE TABLE FacturasFormasPago (
                    Id                INT IDENTITY(1,1) PRIMARY KEY,
                    FacturaId         INT NOT NULL,
                    MetodoPagoCodigo  NVARCHAR(10) NOT NULL,
                    Valor             DECIMAL(18,2) NOT NULL,
                    CONSTRAINT FK_FacturasFormasPago_Facturas FOREIGN KEY (FacturaId)
                        REFERENCES Facturas(Id) ON DELETE CASCADE
                );
                CREATE INDEX IX_FacturasFormasPago_FacturaId ON FacturasFormasPago(FacturaId);
            END");

        context.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AuditoriaAdmin')
            BEGIN
                CREATE TABLE AuditoriaAdmin (
                    Id        INT IDENTITY(1,1) PRIMARY KEY,
                    AdminId   INT NOT NULL,
                    Accion    NVARCHAR(50) NOT NULL,
                    Detalle   NVARCHAR(500) NULL,
                    FechaHora DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                    CONSTRAINT FK_AuditoriaAdmin_Usuarios FOREIGN KEY (AdminId) REFERENCES Usuarios(Id)
                );
                CREATE INDEX IX_AuditoriaAdmin_FechaHora ON AuditoriaAdmin(FechaHora DESC);
                CREATE INDEX IX_AuditoriaAdmin_AdminId   ON AuditoriaAdmin(AdminId);
            END");

        // Columna FacturaId en PosVentas (enlace venta POS → factura electrónica)
        context.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (
                SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = 'PosVentas' AND COLUMN_NAME = 'FacturaId'
            )
            BEGIN
                ALTER TABLE PosVentas ADD FacturaId INT NULL;
            END");

        // Tabla de notificaciones in-app (historial / centro de notificaciones)
        context.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Notificaciones')
            BEGIN
                CREATE TABLE Notificaciones (
                    Id            INT IDENTITY(1,1) PRIMARY KEY,
                    UsuarioId     INT NOT NULL,
                    Tipo          NVARCHAR(20) NOT NULL,
                    Categoria     NVARCHAR(40) NULL,
                    Titulo        NVARCHAR(200) NOT NULL,
                    Mensaje       NVARCHAR(500) NOT NULL,
                    ReferenciaId  INT NULL,
                    Enlace        NVARCHAR(300) NULL,
                    Leida         BIT NOT NULL DEFAULT 0,
                    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                    CONSTRAINT FK_Notificaciones_Usuarios FOREIGN KEY (UsuarioId)
                        REFERENCES Usuarios(Id) ON DELETE CASCADE
                );
                CREATE INDEX IX_Notificaciones_Usuario_Leida
                    ON Notificaciones(UsuarioId, Leida);
                CREATE INDEX IX_Notificaciones_FechaCreacion
                    ON Notificaciones(FechaCreacion DESC);
            END");

        logger.LogInformation("Base de datos lista.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al inicializar la base de datos.");
    }
}

app.MapControllers(); // 5. Finalmente los controllers

app.Run();

