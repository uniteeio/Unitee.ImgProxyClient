using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace DotnetImgProxy
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDotnetImgProxy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(s =>
            {
                var imgProxyBaseUrl = configuration.GetValue<string>("IMG_PROXY_BASE_URL") ?? "https://img.unitee.io";
                return new ImageProxyService(imgProxyBaseUrl);
            });

            return services;
        }
    }
}
