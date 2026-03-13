using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LegalPro.Application;
using LegalPro.Infrastructure;
using LegalPro.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add support for Railway environment variables
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
// CamelCase JSON policy: Token→token, Respuesta→respuesta, etc.
// Esto garantiza compatibilidad con todos los clientes (Android, web).
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Clean Architecture Layers (DDD + CQRS + FluentValidation + Pipeline Behaviours)
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Health Checks for Docker/Kubernetes readiness
builder.Services.AddHealthChecks();

// CORS: orígenes permitidos configurables desde variable de entorno ALLOWED_ORIGINS
// En Railway: ALLOWED_ORIGINS=https://mi-frontend.railway.app,https://legalpro.app
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration["ALLOWED_ORIGINS"]
        ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        ?? [];

    options.AddPolicy("DefaultCors", policy =>
    {
        if (builder.Environment.IsDevelopment() || allowedOrigins.Length == 0)
        {
            // Solo en desarrollo sin configuración se permite cualquier origen
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
    });
});

// Railway environment variables with fallback to appsettings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Override connection string if DATABASE_URL is provided by Railway
if (!string.IsNullOrEmpty(builder.Configuration["DATABASE_URL"]))
{
    connectionString = builder.Configuration["DATABASE_URL"];
}

// Configuration values with Railway environment variable priority
var supabaseUrl = builder.Configuration["SUPABASE_URL"] ?? builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["SUPABASE_SERVICE_KEY"] ?? builder.Configuration["Supabase:ServiceKey"];

// JWT_SECRET NUNCA debe tener fallback con valor fijo — si falta la variable, falla al arrancar
var jwtSecret = builder.Configuration["JWT_SECRET"]
    ?? builder.Configuration["JwtSettings:Secret"]
    ?? throw new InvalidOperationException("JWT_SECRET no está configurado. Configura la variable de entorno en Railway.");

var geminiKey = builder.Configuration["GEMINI_API_KEY"] ?? builder.Configuration["Gemini:ApiKey"];
// Railway usa PORT. En desarrollo local usamos 5000 para no conflictar con altri servicios
var port = builder.Configuration["PORT"] ?? "5000";

// Configure Kestrel to use Railway's assigned port
builder.WebHost.UseUrls($"http://*:{port}");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "LegalProAPI",
            ValidAudience = "LegalProClients",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global Exception Handling (replaces try/catch in every controller)
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("DefaultCors");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Health Check endpoint for Docker/Kubernetes
app.MapHealthChecks("/health");

app.Run();

// Requerido para WebApplicationFactory en integration tests (.NET 9)
public partial class Program { }
