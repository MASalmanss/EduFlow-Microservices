using EduFlow.Shared.Behaviors;
using EduFlow.Shared.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EduFlow.Shared.Extensions;

public static class SharedServiceExtensions
{
    public static IServiceCollection AddSharedServices(
        this IServiceCollection services,
        Assembly assembly)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddHttpContextAccessor();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
