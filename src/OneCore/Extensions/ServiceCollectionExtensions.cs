﻿using Microsoft.Extensions.DependencyInjection;
using OneCore.Hubs;

namespace OneCore.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register core services to DI service container
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IHub, Hub>()
            .AddSingleton(new NewtonSettings())
            .AddKeyedSingleton<ISerializer>("json", (p, _) => new NJsonService(p.GetRequiredService<NewtonSettings>()));
        return services;
    }

    /// <summary>
    /// Removes the first registered service from DI container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection Remove<T>(this IServiceCollection services)
    {
        var service = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
        if (service is not null)
            services.Remove(service);

        return services;
    }
}