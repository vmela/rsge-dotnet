using RsGe.NET.SpecInvoice.Services;
using RsGe.NET.SpecInvoice.Soap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RsGe.NET.SpecInvoice;

/// <summary>
/// Extension methods for registering RS.GE SpecInvoice services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register RS.GE SpecInvoice services using configuration from appsettings.json
    /// </summary>
    public static IServiceCollection AddRsGeSpecInvoiceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("RsGe:SpecInvoice").Get<SpecInvoiceConfiguration>()
                     ?? new SpecInvoiceConfiguration();
        return services.AddRsGeSpecInvoiceServices(config);
    }

    /// <summary>
    /// Register RS.GE SpecInvoice services using configuration object
    /// </summary>
    public static IServiceCollection AddRsGeSpecInvoiceServices(this IServiceCollection services, SpecInvoiceConfiguration config)
    {
        services.AddSingleton(config);
        services.AddHttpClient<ISpecInvoiceSoapClient, SpecInvoiceSoapClient>();

        services.AddScoped<IRsGeSpecInvoiceService>(sp =>
        {
            var soapClient = sp.GetRequiredService<ISpecInvoiceSoapClient>();
            return new RsGeSpecInvoiceService(soapClient, config.ServiceUser, config.ServicePassword, config.UserId);
        });

        return services;
    }

    /// <summary>
    /// Register RS.GE SpecInvoice services using configuration delegate
    /// </summary>
    public static IServiceCollection AddRsGeSpecInvoiceServices(this IServiceCollection services, Action<SpecInvoiceConfiguration> configure)
    {
        var config = new SpecInvoiceConfiguration();
        configure(config);
        return services.AddRsGeSpecInvoiceServices(config);
    }
}
