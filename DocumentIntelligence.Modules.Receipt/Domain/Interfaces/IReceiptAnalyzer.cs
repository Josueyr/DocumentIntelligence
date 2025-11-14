using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DocumentIntelligence.Common.Models;
using DocumentIntelligence.Modules.Receipt.Domain.Models;

namespace DocumentIntelligence.Modules.Receipt.Domain.Interfaces
{
    public interface IReceiptAnalyzer
    {
        Task<OperationResult<List<ReceiptResult>>> AnalyzeAsync(Stream fileStream, CancellationToken cancellationToken = default);
    }
}
