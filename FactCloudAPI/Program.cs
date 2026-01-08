using   FactCloudAPI.Data;
using FactCloudAPI.Services;
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

// ===== Base de datos =====
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(conn, sqlServerOptions =>
    {
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
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

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddHostedService<UsuariosDesactivadosService>();

var app = builder.Build();

// ===== Middleware ordenado =====

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  // 1. Primero redirección HTTPS
app.UseCors("AllowReact");  // 2. Luego CORS
app.UseAuthentication();    // 3. Autenticación (lee el token)
app.UseAuthorization();     // 4. Autorización (verifica permisos)
app.MapHub<NotificacionesHub>("/notificacionesHub").AllowAnonymous();

app.MapControllers();       // 5. Finalmente los controllers

app.Run();
