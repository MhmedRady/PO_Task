using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Domain.BuildingBlocks;

public class StatusTransitionStrategyException : Exception
{
    public StatusTransitionStrategyException(PurchaseOrderStatus status) : base($"No strategy found for status {status}")
    {
    }
}
