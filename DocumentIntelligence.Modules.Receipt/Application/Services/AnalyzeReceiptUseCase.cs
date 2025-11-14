using DocumentIntelligence.Common.Models;
using DocumentIntelligence.Modules.Receipt.Application.Interfaces;
using DocumentIntelligence.Modules.Receipt.Domain.Interfaces;
using DocumentIntelligence.Modules.Receipt.Domain.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace DocumentIntelligence.Modules.Receipt.Application.Services
{
    public class AnalyzeReceiptUseCase : IAnalyzeReceiptUseCase
    {
        private readonly IReceiptAnalyzer _analyzer;

        public AnalyzeReceiptUseCase(IReceiptAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        public async Task<OperationResult<List<ReceiptResult>>> AnalyzeReceiptAsync(Stream fileStream, CancellationToken cancellationToken = default)
        {
            return await _analyzer.AnalyzeAsync(fileStream, cancellationToken);
        }
    }
}
