using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.PurchaseOrders;

public static class PurchaseOrderErrors
{
    public static readonly Error NotFound = new(
        "PurchaseOrder.NotFound",
        "The PurchaseOrder was not found");

    public static readonly Error MultipleCurrencyTypes = new(
        "OrderItems.PriceError",
        "The Multiple Price Currency Types");
}
