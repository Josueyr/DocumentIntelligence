public static class ReceiptModuleDependencyInjection
{
    public static IServiceCollection AddReceiptModule(this IServiceCollection services)
    {
        services
            .AddApplication()
            .AddInfrastructure();

        return services;
    }
}
