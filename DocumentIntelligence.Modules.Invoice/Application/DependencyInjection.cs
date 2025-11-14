using DocumentIntelligence.Modules.Invoice.Application.Interfaces;
using DocumentIntelligence.Modules.Invoice.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentIntelligence.Modules.Invoice.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddInvoiceApplication(this IServiceCollection services)
        {
            services.AddScoped<IAnalyzeInvoiceUseCase, AnalyzeInvoiceUseCase>();
            return services;
        }
    }
}
