using DocumentIntelligence.Modules.Receipt.Domain.Interfaces;
using DocumentIntelligence.Modules.Receipt.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IReceiptAnalyzer, AzureReceiptAnalyzer>();
        return services;
    }
}
