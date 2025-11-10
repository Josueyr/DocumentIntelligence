using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DocumentIntelligence.Modules.Receipt.Domain.Models;

namespace DocumentIntelligence.Modules.Receipt.Application.Interfaces
{
    public interface IAnalyzeReceiptUseCase
    {
        Task<OperationResult<List<ReceiptResult>>> AnalyzeReceiptAsync(Stream fileStream, CancellationToken cancellationToken = default);
    }
}
