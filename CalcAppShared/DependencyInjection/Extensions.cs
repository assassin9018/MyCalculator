using Microsoft.Extensions.DependencyInjection;

namespace CalcAppShared.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddSmartCalcModels(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddSmartCalcViewModels(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddSmartCalcServices(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddSmartCalc(this IServiceCollection services)
    {
        return services.AddSmartCalcModels()
            .AddSmartCalcViewModels()
            .AddSmartCalcServices();
    }
}
