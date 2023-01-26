using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

namespace Unitee.ImgProxyClient;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddImgProxyClient(this IServiceCollection services, IConfiguration configuration, string? commonOptions = null)
    {
        services.AddScoped(s =>
        {
            var imgProxyBaseUrl = configuration["ImgProxy:BaseUrl"];
            var key = configuration["ImgProxy:Key"];
            var salt = configuration["ImgProxy:Salt"];

            if (imgProxyBaseUrl is null)
            {
                throw new Exception("Please provide a value for ImgProxy:BaseUrl in the configuration");
            }

            return new ImgProxyService(imgProxyBaseUrl, key, salt, commonOptions);
        });

        return services;
    }
}
