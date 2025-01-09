using PO_Task.Application.PurchaseOrders;
using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Api.Controllers;

public sealed record AddPurchaseOrderRequest(
    PoNumberGeneratorType PONumberType,
    string PriceCurrencyCode,
    IReadOnlyList<AddPurchaseOrderItemRequest> PurchaseOrderItems
    )
{
    public static implicit operator AddPurchaseOrderCommand(AddPurchaseOrderRequest request)
    {
        return new AddPurchaseOrderCommand(
                request.PONumberType,
                request.PurchaseOrderItems.Select(poItem =>
                new PurchaseOrderItemCommand(
                    poItem.GoodCode,
                    poItem.Quantity,
                    poItem.Price,
                    request.PriceCurrencyCode
                )
            ).ToArray()
        );
    }
}

public sealed record AddPurchaseOrderItemRequest(
    string GoodCode,
    int SerialNumber,
    decimal Quantity,
    decimal Price);
