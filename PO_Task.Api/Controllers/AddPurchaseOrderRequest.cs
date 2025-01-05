using PO_Task.Application.PurchaseOrders;

namespace PO_Task.Api.Controllers;

public sealed record AddPurchaseOrderRequest(
    Guid PurchaserId,
    PurchaseOrderItemRequest PurchaseOrderItem
    )
{
    public static implicit operator AddPurchaseOrderCommand(AddPurchaseOrderRequest request)
    {
        return new AddPurchaseOrderCommand(
            request.PurchaserId,
            new PurchaseOrderItemCommand(
                Guid.NewGuid(),
                request.PurchaseOrderItem.GoodCode,
                request.PurchaseOrderItem.SerialNumber,
                request.PurchaseOrderItem.Quantity,
                request.PurchaseOrderItem.Price,
                request.PurchaseOrderItem.PriceCurrencyCode
            )
        );
    }
}

public sealed record PurchaseOrderItemRequest(
    string GoodCode,
    int SerialNumber,
    decimal Quantity,
    decimal Price,
    string PriceCurrencyCode);
