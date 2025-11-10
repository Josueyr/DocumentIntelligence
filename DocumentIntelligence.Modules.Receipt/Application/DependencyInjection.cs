using DocumentIntelligence.Modules.Receipt.Application.Interfaces;
using DocumentIntelligence.Modules.Receipt.Application.Services;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAnalyzeReceiptUseCase, AnalyzeReceiptUseCase>();
        return services;
    }
}
