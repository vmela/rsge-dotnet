using RsGe.NET.Core;
using RsGe.NET.WayBill.Services;
using RsGe.NET.WayBill.Soap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RsGe.NET.WayBill;

/// <summary>
/// Extension methods for registering RS.GE WayBill services with dependency injection
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRsGeWayBillServices(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(RsGeConfiguration.SectionName).Get<WayBillConfiguration>()
                     ?? new WayBillConfiguration();

        return services.AddRsGeWayBillServices(config);
    }

    public static IServiceCollection AddRsGeWayBillServices(this IServiceCollection services, WayBillConfiguration config)
    {
        services.AddSingleton<RsGeConfiguration>(config);
        services.AddSingleton(config);

        services.AddHttpClient<IWayBillSoapClient, WayBillSoapClient>();

        services.AddScoped<IRsGeDynamicServiceFactory>(sp =>
        {
            var soapClient = sp.GetRequiredService<IWayBillSoapClient>();
            return new RsGeDynamicServiceFactory(soapClient);
        });

        services.AddScoped<IRsGeWayBillService>(sp =>
        {
            var soapClient = sp.GetRequiredService<IWayBillSoapClient>();
            return new RsGeWayBillService(soapClient, config.ServiceUser, config.ServicePassword);
        });

        return services;
    }

    public static IServiceCollection AddRsGeWayBillServices(this IServiceCollection services, Action<WayBillConfiguration> configure)
    {
        var config = new WayBillConfiguration();
        configure(config);

        return services.AddRsGeWayBillServices(config);
    }
}
