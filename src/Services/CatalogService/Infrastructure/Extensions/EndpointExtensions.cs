using CatalogService.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace CatalogService.Infrastructure.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var descriptors = assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false } &&
                        t.IsAssignableTo(typeof(IEndpoint)))
            .Select(t => ServiceDescriptor.Transient(typeof(IEndpoint), t))
            .ToArray();

        services.TryAddEnumerable(descriptors);
        return services;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        foreach (var endpoint in endpoints)
            endpoint.MapEndpoint(app);
        return app;
    }
}
