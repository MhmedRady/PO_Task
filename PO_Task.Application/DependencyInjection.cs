using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PO_Task.Application.Abstractions.Behaviors;
using PO_Task.Application.Orders;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.PurchaseOrders;
using System.Reflection;

namespace PO_Task.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(
            configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));

                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

        services.AddValidatorsFromAssembly(
                    typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        AddStrategies(services);

        services.AddScoped<IStatusTransitionStrategyFactory<PurchaseOrder, PurchaseOrderStatus>, StatusTransitionStrategyFactory>();

        return services;
    }

    private static void AddStrategies(IServiceCollection services)
    {
        Assembly assembly = typeof(IStatusTransitionStrategy<,>).Assembly;

        Type strategyType = typeof(IStatusTransitionStrategy<PurchaseOrder, PurchaseOrderStatus>);

        IEnumerable<Type> types = assembly.GetTypes().Where(
            t => strategyType.IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false }
        );

        foreach (Type type in types)
        {
            Type? interfaceType = type.GetInterface(strategyType.Name);
            if (interfaceType != null)
            {
                services.AddScoped(
                    interfaceType,
                    type);
            }
        }
    }
}
