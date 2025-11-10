using Azure;
using Azure.AI.DocumentIntelligence;
using DocumentIntelligence.Modules.Receipt.Domain.Interfaces;
using DocumentIntelligence.Modules.Receipt.Domain.Models;
using DocumentIntelligence.Modules.Receipt.Infrastructure.Config;
using DocumentIntelligence.Modules.Receipt.Infrastructure.Services.Mappers;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentIntelligence.Modules.Receipt.Infrastructure.Services
{
    public class AzureReceiptAnalyzer : IReceiptAnalyzer
    {
        private readonly AzureSettingsModel _settings;

        public AzureReceiptAnalyzer(IOptions<AzureSettingsModel> settings)
        {
            _settings = settings.Value;
        }

        public async Task<OperationResult<List<ReceiptResult>>> AnalyzeAsync(Stream fileStream, CancellationToken cancellationToken = default)
        {
            try
            {
                var credential = new AzureKeyCredential(_settings.ApiKey);
                var client = new DocumentIntelligenceClient(new Uri(_settings.Endpoint), credential);

                AnalyzeDocumentOptions options = new("prebuilt-receipt", BinaryData.FromStream(fileStream));
                options.Features.Add(DocumentAnalysisFeature.QueryFields);
                options.QueryFields.Add("InvoiceId");
                options.QueryFields.Add("MerchantCif");

                var operation = await client.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    options,
                    cancellationToken
                );

                var result = operation.Value;

                if (result == null || result.Documents.Count == 0)
                {
                    return OperationResult<List<ReceiptResult>>.Failure("No se pudo analizar el recibo.");
                }

                return AzureReceiptMapper.MapFrom(result.Documents);

            }
            catch(RequestFailedException ex)
            {
                return OperationResult<List<ReceiptResult>>.Failure($"Error al analizar el documento: {ex.Message}");
            }
        }
    }
}
