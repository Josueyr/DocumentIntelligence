using DocumentIntelligence.Common.Models;
using DocumentIntelligence.Modules.Invoice.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentIntelligence.Modules.Invoice.Domain.Interfaces
{
    public interface IInvoiceAnalyzer
    {
        Task<OperationResult<List<InvoiceResult>>> AnalyzeAsync(Stream fileStream, CancellationToken cancellationToken = default);
    }
}
