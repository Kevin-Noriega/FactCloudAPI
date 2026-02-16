using   FactCloudAPI.Data;
using FactCloudAPI.Services;
using FactCloudAPI.Services.AuthLogin;
using FactCloudAPI.Services.Clientes;
using FactCloudAPI.Services.Facturas;
using FactCloudAPI.Services.Productos;
using FactCloudAPI.Services.Seguridad;
using FactCloudAPI.Services.Usuarios;
using FactCloudAPI.Services.Wompi;
using FactCloudAPI.Utils.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


var builder = WebApplication.CreateBuilder(args);

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
                origin.StartsWith("http://localhost") ||
                origin.StartsWith("https://localhost"))
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ===== Controllers y JSON =====
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// ===== Swagger =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddScoped<SeguridadService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IFacturaService, FacturaService>();
builder.Services.AddHttpClient<WompiService>();
builder.Services.AddScoped<WompiService>();
builder.Services.AddScoped<IDocumentoSoporteService, DocumentoSoporteService>();
builder.Services.AddHttpClient<WompiService>(client =>
{
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});


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


    // ===== JWT Config =====
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key missing"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
var app = builder.Build();
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
                mensaje = "Error interno del servidor"
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



app.UseHttpsRedirection();  // 1. Primero redirección HTTPS
app.UseCors("AllowReact");  // 2. Luego CORS
app.UseAuthentication();    // 3. Autenticación (lee el token)
app.UseAuthorization();     // 4. Autorización (verifica permisos)
app.MapHub<NotificacionesHub>("/api/notificacionesHub").AllowAnonymous();

app.MapControllers();       // 5. Finalmente los controllers

app.Run();
