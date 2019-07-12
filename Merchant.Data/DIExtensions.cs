using Merchant.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Merchant.Data
{
    public static class DIExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddSingleton<Repository>();
            return services;
        }
    }
}
