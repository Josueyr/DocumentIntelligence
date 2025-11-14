using DocumentIntelligence.Common.Models.AzureSettings;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios y serialización JSON.
// Aquí se definen las opciones de cómo la API va a devolver JSON
// (por ejemplo, omitir valores nulos y formatear con indentación para facilitar lectura).
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra los módulos de DocumentIntelligence (inyección de dependencias para los módulos).
builder.Services.AddDocumentIntelligenceModules();

// Configuración de credenciales y endpoint de Azure desde appsettings.
// Leemos la sección "AzureSettings" para poder conectar con Azure más tarde.
builder.Services.Configure<AzureSettingsModel>(
    builder.Configuration.GetSection("AzureSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares básicos: HTTPS, autorización y mapeo de controladores.
// La pipeline mínima para exponer los endpoints REST.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();