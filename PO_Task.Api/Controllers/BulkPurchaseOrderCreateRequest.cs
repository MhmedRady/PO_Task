using PO_Task.Application.PurchaseOrders;

namespace PO_Task.Api.Controllers;

public sealed record BulkPurchaseOrderCreateRequest(
    Guid PurchaserId,
    DateTime IssueDate,
    IEnumerable<BulkPurchaseOrderItemRequest> PurchaseOrderItems
    )
{
    public static implicit operator BulkPurchaseOrderCreateCommand(BulkPurchaseOrderCreateRequest request)
    {
        return new BulkPurchaseOrderCreateCommand(
            request.PurchaserId,
            request.IssueDate,
            request.PurchaseOrderItems.Select(item => new BulkOrderItemsCreateCommand(
                Guid.NewGuid(),
                item.GoodCode,
                item.SerialNumber,
                item.Quantity,
                item.Price,
                item.PriceCurrencyCode
            )));   
    }
}

public sealed record BulkPurchaseOrderItemRequest(
    string GoodCode,
    int SerialNumber,
    decimal Quantity,
    decimal Price,
    string PriceCurrencyCode);
