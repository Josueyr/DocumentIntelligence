using DocumentIntelligence.Modules.Receipt.Application.Interfaces;
using DocumentIntelligence.Modules.Receipt.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentIntelligence.Modules.Receipt.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddReceiptApplication(this IServiceCollection services)
        {
            services.AddScoped<IAnalyzeReceiptUseCase, AnalyzeReceiptUseCase>();
            return services;
        }
    }
}
