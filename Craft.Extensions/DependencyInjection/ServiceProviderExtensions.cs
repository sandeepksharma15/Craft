#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class ServiceProviderExtensions
{
    /// <summary>
    /// Adds a service to the dependency injection container with the specified service lifetime.
    /// Provides a concise syntax for registering services with different lifetimes.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationType">The type of the concrete implementation to register.</param>
    /// <param name="lifetime">The desired lifetime of the service (Transient, Scoped, or Singleton).</param>
    /// <returns>The IServiceCollection with the added service.</returns>
    /// <exception cref="ArgumentException">Thrown if an invalid lifetime is specified.</exception>
    public static IServiceCollection AddService(this IServiceCollection services, Type serviceType,
        Type implementationType, ServiceLifetime lifetime)
    {
        return lifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
            _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
        };
    }

    /// <summary>
    /// Automatically registers all concrete implementations of a specified interface within the current domain as services in the dependency injection container.
    /// Uses reflection to discover and register services, promoting convention-based registration and reducing manual setup.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <param name="interfaceType">The type of the interface to register implementations for.</param>
    /// <param name="lifetime">The desired lifetime of the registered services.</param>
    /// <returns>The IServiceCollection with the added services.</returns>
    public static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
    {
        var interfaceTypes =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => interfaceType.IsAssignableFrom(t)
                            && t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterfaces().FirstOrDefault(),
                    Implementation = t
                })
                .Where(t => t.Service is not null
                            && interfaceType.IsAssignableFrom(t.Service));

        foreach (var type in interfaceTypes)
            services.AddService(type.Service!, type.Implementation, lifetime);

        return services;
    }

    /// <summary>
    /// Retrieves a singleton instance of the specified service type from the dependency injection container,
    /// throwing an exception if the service is not registered as a singleton.
    /// Provides a way to access singletons directly from the service collection.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve.</typeparam>
    /// <param name="services">The IServiceCollection to get the service from.</param>
    /// <returns>The singleton instance of the service.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the service is not registered as a singleton.</exception>
    public static T GetSingletonInstance<T>(this IServiceCollection services)
    {
        var service = services.GetSingletonInstanceOrNull<T>();

        return service == null
            ? throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName)
            : service;
    }

    /// <summary>
    /// Retrieves a singleton instance of the specified service type from the dependency injection container,
    /// returning null if the service is not registered or is not a singleton.
    /// Allows for optional retrieval of singletons without throwing exceptions.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve.</typeparam>
    /// <param name="services">The IServiceCollection to get the service from.</param>
    /// <returns>The singleton instance of the service, or null if not found or not registered as a singleton.</returns>
    public static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
    {
        return (T)services
            .FirstOrDefault(d => d.ServiceType == typeof(T))
            ?.ImplementationInstance;
    }

    /// <summary>
    /// Determines whether a service of the specified type has been added to the dependency injection container.
    /// Provides a convenient way to check service registration without accessing the container directly.
    /// </summary>
    /// <typeparam name="T">The type of the service to check for registration.</typeparam>
    /// <param name="services">The IServiceCollection to check.</param>
    /// <returns>True if the service is registered, false otherwise.</returns>
    public static bool IsAdded<T>(this IServiceCollection services)
        => services.IsAdded(typeof(T));

    /// <summary>
    /// Determines whether a service of the specified type has been added to the dependency injection container.
    /// Offers flexibility by checking for registration using a Type instance directly.
    /// </summary>
    /// <param name="services">The IServiceCollection to check.</param>
    /// <param name="type">The type of the service to check for registration.</param>
    /// <returns>True if the service is registered, false otherwise.</returns>
    public static bool IsAdded(this IServiceCollection services, Type type)
        => services.Any(d => d.ServiceType == type);

    /// <summary>
    /// Determines whether the concrete implementation type of the specified service type has been added to the dependency injection container.
    /// Focuses specifically on the implementation type, ensuring it's registered regardless of interface mappings.
    /// </summary>
    /// <typeparam name="T">The type of the service to check for implementation registration.</typeparam>
    /// <param name="services">The IServiceCollection to check.</param>
    /// <returns>True if the implementation type is registered, false otherwise.</returns>
    public static bool IsImplementationAdded<T>(this IServiceCollection services)
        => services.IsImplementationAdded(typeof(T));

    /// <summary>
    /// Determines whether the concrete implementation type of the specified Type has been added to the dependency injection container.
    /// Offers flexibility by checking for implementation registration using a Type instance directly.
    /// </summary>
    /// <param name="services">The IServiceCollection to check.</param>
    /// <param name="type">The implementation type to check for registration.</param>
    /// <returns>True if the implementation type is registered, false otherwise.</returns>
    public static bool IsImplementationAdded(this IServiceCollection services, Type type)
        => services.Any(d => d.ImplementationType == type);

    /// <summary>
    /// Resolves and creates an instance of the specified service type using dependency injection,
    /// allowing for injection of constructor parameters.
    /// Simplifies instance creation with constructor dependencies within a dependency injection context.
    /// </summary>
    /// <typeparam name="T">The type of the service to resolve.</typeparam>
    /// <param name="provider">The IServiceProvider to use for dependency resolution.</param>
    /// <param name="parameters">The constructor parameters to inject into the service instance.</param>
    /// <returns>The resolved instance of the service.</returns>
    public static T ResolveWith<T>(this IServiceProvider provider, params object[] parameters) where T : class
        => ActivatorUtilities.CreateInstance<T>(provider, parameters);
}
