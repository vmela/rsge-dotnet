using RsGe.NET.TaxPayer.Services;
using RsGe.NET.TaxPayer.Soap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RsGe.NET.TaxPayer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRsGeTaxPayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("RsGe:TaxPayer").Get<TaxPayerConfiguration>()
                     ?? new TaxPayerConfiguration();
        return services.AddRsGeTaxPayerServices(config);
    }

    public static IServiceCollection AddRsGeTaxPayerServices(this IServiceCollection services, TaxPayerConfiguration config)
    {
        services.AddSingleton(config);
        services.AddHttpClient<ITaxPayerSoapClient, TaxPayerSoapClient>();

        services.AddScoped<IRsGeTaxPayerService>(sp =>
        {
            var soapClient = sp.GetRequiredService<ITaxPayerSoapClient>();
            return new RsGeTaxPayerService(soapClient, config.Username, config.Password);
        });

        return services;
    }

    public static IServiceCollection AddRsGeTaxPayerServices(this IServiceCollection services, Action<TaxPayerConfiguration> configure)
    {
        var config = new TaxPayerConfiguration();
        configure(config);
        return services.AddRsGeTaxPayerServices(config);
    }
}
