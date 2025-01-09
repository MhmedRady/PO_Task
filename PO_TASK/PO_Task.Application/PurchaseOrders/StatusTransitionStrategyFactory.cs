

using Microsoft.Extensions.DependencyInjection;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Application.Orders;

public class StatusTransitionStrategyFactory : IStatusTransitionStrategyFactory<PurchaseOrder, PurchaseOrderStatus>
{
    private readonly IServiceProvider _serviceProvider;

    public StatusTransitionStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IStatusTransitionStrategy<PurchaseOrder, PurchaseOrderStatus> GetStrategy(PurchaseOrderStatus status)
    {
        return _serviceProvider.GetServices<IStatusTransitionStrategy<PurchaseOrder, PurchaseOrderStatus>>()
            .FirstOrDefault(s => s.From == status) ?? throw new StatusTransitionStrategyException(status);
    }
}
