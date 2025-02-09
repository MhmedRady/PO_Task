using PO_Task.Application.PurchaseOrders;
using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Api.Controllers;

public sealed record BulkPurchaseOrderCreateRequest(

    IReadOnlyList<BulkPurchaseOrderRequest> PurchaseOrderRequests
    )
{
    public static implicit operator BulkPurchaseOrderCreateCommand(BulkPurchaseOrderCreateRequest request)
    {
        return new BulkPurchaseOrderCreateCommand(
                request.PurchaseOrderRequests.Select(poReuest =>
                        new BulkPurchaseOrderCommand(
                            poReuest.PoNumberType,
                            PO_Items: poReuest.PurchaseOrderItems.Select(poItemRequest =>
                                    new BulkPurchaseOrderItemCreateCommand(
                                            poItemRequest.GoodCode,
                                            poItemRequest.Quantity,
                                            poItemRequest.Price,
                                            poReuest.PriceCurrencyCode
                                        )
                                )
                            )
                    ).ToArray()
            );
    }
}

public sealed record BulkPurchaseOrderRequest(
        PoNumberGeneratorType PoNumberType,
        string PriceCurrencyCode,
        IEnumerable<BulkPurchaseOrderItemRequest> PurchaseOrderItems
    );

public sealed record BulkPurchaseOrderItemRequest(
    string GoodCode,
    decimal Quantity,
    decimal Price);
