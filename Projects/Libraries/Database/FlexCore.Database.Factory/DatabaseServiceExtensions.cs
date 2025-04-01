using FlexCore.Database.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FlexCore.Database.Factory.Extensions
{
    public static class DatabaseServiceExtensions
    {
        public static IServiceCollection AddDatabaseProvider(
            this IServiceCollection services,
            string providerName,
            string connectionString)
        {
            services.AddSingleton<IDbConnectionFactory>(sp =>
            {
                var factory = sp.GetRequiredService<IDatabaseProviderFactory>();
                return factory.CreateProvider(providerName, connectionString);
            });

            return services;
        }
    }
}