using DocumentIntelligence.Common.Models;
using DocumentIntelligence.Modules.Invoice.Application.Interfaces;
using DocumentIntelligence.Modules.Invoice.Domain.Interfaces;
using DocumentIntelligence.Modules.Invoice.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentIntelligence.Modules.Invoice.Application.Services
{
    public class AnalyzeInvoiceUseCase : IAnalyzeInvoiceUseCase
    {
        private readonly IInvoiceAnalyzer _analyzer;

        public AnalyzeInvoiceUseCase(IInvoiceAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        public async Task<OperationResult<List<InvoiceResult>>> AnalyzeInvoiceAsync(Stream fileStream, CancellationToken cancellationToken = default)
        {
            return await _analyzer.AnalyzeAsync(fileStream, cancellationToken);
        }
    }
}
