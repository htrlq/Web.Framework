using Consul;
using Distriobuted.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Distriobuted.Consul
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddConsulClient(this IServiceCollection services, Action<ConsulClientConfiguration> action)
        {
            var config = new ConsulClientConfiguration();
            action.Invoke(config);

            services.TryAddSingleton<ConsulConfiguration>();
            services.TryAddSingleton<IConfiguration<ConsulClientConfiguration>>(serviceProvider =>
            {
                var service = serviceProvider.GetRequiredService<ConsulConfiguration>();
                service.SetConfiguration(config);

                return service;
            });

            return services;
        }

        public static IServiceCollection AddConsulConfig(this IServiceCollection services)
        {
            services.TryAddSingleton<IDistributedConfigruation, ConsulDistributedConfigruation>();

            return services;
        }

        public static IServiceCollection AddConsulCache(this IServiceCollection services)
        {
            services.TryAddSingleton<IKvCache, ConsulKvCache>();

            return services;
        }

        public static IServiceCollection ConfigConsul<TEntity>(this IServiceCollection services, string key)
            where TEntity:class,new()
        {
            services.TryAddSingleton<IOptionsFactory<TEntity>, ConsulKvOptionFactory<TEntity>>();

            services.TryAddScoped<IOptions<TEntity>>(serviceProvider =>
            {
                using (var serviceScope = serviceProvider.CreateScope())
                {
                    var factory = serviceScope.ServiceProvider.GetRequiredService<IOptionsFactory<TEntity>>();
                    var entity = factory.Create(key);

                    return new ConsulKvOptions<TEntity>(entity);
                }
            });

            return services;
        }
    }
}

