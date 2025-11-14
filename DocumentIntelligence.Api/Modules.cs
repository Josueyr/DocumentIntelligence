using DocumentIntelligence.Modules.Receipt.Application;
using DocumentIntelligence.Modules.Receipt.Infrastructure;
using DocumentIntelligence.Modules.Invoice.Application;
using DocumentIntelligence.Modules.Invoice.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

public static class DocumentIntelligenceModulesDependencyInjection
{
    public static IServiceCollection AddReceiptModule(this IServiceCollection services)
    {
        services
            .AddReceiptApplication()
            .AddReceiptInfrastructure();

        return services;
    }

    public static IServiceCollection AddInvoiceModule(this IServiceCollection services)
    {
        services
            .AddInvoiceApplication()
            .AddInvoiceInfrastructure();

        return services;
    }

    public static IServiceCollection AddDocumentIntelligenceModules(this IServiceCollection services)
    {
        services.AddReceiptModule();
        services.AddInvoiceModule();

        return services;
    }
}
