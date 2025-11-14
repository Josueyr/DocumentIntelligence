using DocumentIntelligence.Modules.Receipt.Domain.Interfaces;
using DocumentIntelligence.Modules.Receipt.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentIntelligence.Modules.Receipt.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddReceiptInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IReceiptAnalyzer, AzureReceiptAnalyzer>();
            return services;
        }
    }
}
