using DocumentIntelligence.Common.Models.AzureSettings;
using DocumentIntelligence.Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Text;
using DocumentIntelligence.Api.Auth.Services;
using DocumentIntelligence.Api.Auth.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios y serialización JSON.
// Aquí se definen las opciones de cómo la API va a devolver JSON
// (por ejemplo, omitir valores nulos y formatear con indentación para facilitar lectura).
builder.Services.AddControllers(options =>
{
    // Requiere autorización por defecto en todos los endpoints. Los controladores o acciones
    // que deban permitir acceso anónimo deben usar [AllowAnonymous].
    options.Filters.Add(new AuthorizeFilter());
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger con soporte para JWT Bearer
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DocumentIntelligence API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce 'Bearer' seguido de un espacio y tu token válido en el campo.\r\n\r\nEjemplo: 'Bearer abcdef12345'",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});

// Registra los módulos de DocumentIntelligence (inyección de dependencias para los módulos).
builder.Services.AddDocumentIntelligenceModules();

// Configuración de credenciales y endpoint de Azure desde appsettings.
// Leemos la sección "AzureSettings" para poder conectar con Azure más tarde.
builder.Services.Configure<AzureSettingsModel>(
    builder.Configuration.GetSection("AzureSettings"));

// Configurar ajustes JWT y autenticación
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();

builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key ?? string.Empty)),
        ValidateIssuer = !string.IsNullOrEmpty(jwtSettings.Issuer),
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = !string.IsNullOrEmpty(jwtSettings.Audience),
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
    };
});

// Registrar servicios de autorización (pueden ampliarse con políticas)
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares básicos: HTTPS, autenticación, autorización y mapeo de controladores.
// La pipeline mínima para exponer los endpoints REST.
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();