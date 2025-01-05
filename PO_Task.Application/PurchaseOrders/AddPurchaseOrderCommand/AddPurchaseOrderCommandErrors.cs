using PO_Task.Application.Exceptions;

namespace PO_Task.Application.PurchaseOrders;

public static class AddPurchaseOrderCommandErrors
{
    public static readonly ValidationError PurchaserIdNotFound = new(
        $"{nameof(AddPurchaseOrderCommand)}.Purchaser UserId",
        "For adding a Order, the Purchaser UserId must be existing.");
}
