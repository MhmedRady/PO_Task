using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.Orders;

public static class PurchaseOrderErrors
{
    public static readonly Error NotFound = new(
        "OrdersLine.NotFound",
        "The OrdersLine was not found");
}
