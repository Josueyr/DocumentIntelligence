using Azure;
using Azure.AI.DocumentIntelligence;
using DocumentIntelligence.Common.Models;
using DocumentIntelligence.Common.Models.AzureSettings;
using DocumentIntelligence.Modules.Invoice.Domain.Interfaces;
using DocumentIntelligence.Modules.Invoice.Domain.Models;
using DocumentIntelligence.Modules.Invoice.Infrastructure.Services.Mappers;
using Microsoft.Extensions.Options;

namespace DocumentIntelligence.Modules.Invoice.Infrastructure.Services
{
    // Servicio que sabe comunicarse con Azure Document Intelligence
    // para analizar una factura y devolver nuestro modelo interno.
    // Encapsula toda la lógica necesaria para llamar
    // al servicio de Azure y transformar la respuesta en algo usable.
    public class AzureInvoiceAnalyzer : IInvoiceAnalyzer
    {
        private readonly AzureSettingsModel _settings;

        public AzureInvoiceAnalyzer(IOptions<AzureSettingsModel> settings)
        {
            _settings = settings.Value;
        }

        // Analiza el stream de la factura usando el modelo preconstruido de "invoice".
        // Paso a paso: crea el cliente de Azure, envía el archivo, espera el resultado
        // y lo mapea a `InvoiceResult`. Maneja errores de petición devolviendo
        // un `OperationResult` con mensaje amigable.
        public async Task<OperationResult<List<InvoiceResult>>> AnalyzeAsync(Stream fileStream, CancellationToken cancellationToken = default)
        {
            try
            {
                var credential = new AzureKeyCredential(_settings.ApiKey);
                var client = new DocumentIntelligenceClient(new Uri(_settings.Endpoint), credential);

                AnalyzeDocumentOptions options = new("prebuilt-invoice", BinaryData.FromStream(fileStream));

                var operation = await client.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    options,
                    cancellationToken
                );

                var result = operation.Value;

                if (result == null || result.Documents.Count == 0)
                {
                    return OperationResult<List<InvoiceResult>>.Failure("No se pudo analizar la factura.");
                }

                return AzureInvoiceMapper.MapFrom(result.Documents);
            }
            catch (RequestFailedException ex)
            {
                return OperationResult<List<InvoiceResult>>.Failure($"Error al analizar el documento: {ex.Message}");
            }
        }
    }
}
