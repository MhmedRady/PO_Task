using PO_Task.Application.Exceptions;

namespace PO_Task.Application.PurchaseOrders;

public static class UpdatePurchaseOrderItemCommandErrors
{
    public static readonly ValidationError PurchaserIdNotFound = new(
        $"{nameof(PurchaseOrderItemCommand)}.Purchaser UserId",
        "For adding a Order, the Purchaser UserId must be existing.");
}
