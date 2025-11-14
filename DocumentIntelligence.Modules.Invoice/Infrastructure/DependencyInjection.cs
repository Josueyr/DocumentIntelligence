using DocumentIntelligence.Modules.Invoice.Domain.Interfaces;
using DocumentIntelligence.Modules.Invoice.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentIntelligence.Modules.Invoice.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInvoiceInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IInvoiceAnalyzer, AzureInvoiceAnalyzer>();
            return services;
        }
    }
}
