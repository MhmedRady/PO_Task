using PO_Task.Application.PurchaseOrders;

namespace PO_Task.Api.Controllers;

public sealed record AddPurchaseOrderItemRequest(
        string PoNumber,
        string GoodCode,
        //int SerialNumber,
        decimal Quantity,
        decimal Price,
        string PriceCurrencyCode
    )
{
    public static implicit operator AddPurchaseOrderItemCommand(AddPurchaseOrderItemRequest request)
    {
        return new AddPurchaseOrderItemCommand(
            request.PoNumber,
            request.GoodCode,
            //request.SerialNumber,
            request.Quantity,
            request.Price,
            request.PriceCurrencyCode
        );
    }
}