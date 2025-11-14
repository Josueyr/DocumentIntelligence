using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DocumentIntelligence.Common.Models;
using DocumentIntelligence.Modules.Invoice.Domain.Models;

namespace DocumentIntelligence.Modules.Invoice.Application.Interfaces
{
    public interface IAnalyzeInvoiceUseCase
    {
        Task<OperationResult<List<InvoiceResult>>> AnalyzeInvoiceAsync(Stream fileStream, CancellationToken cancellationToken = default);
    }
}
