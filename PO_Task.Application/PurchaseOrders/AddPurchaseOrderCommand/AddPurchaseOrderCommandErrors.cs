using PO_Task.Application.Exceptions;

namespace PO_Task.Application.PurchaseOrders;

public static class AddPurchaseOrderCommandErrors
{
    public static readonly ApplicationError PurchaserIdNotFound = new(
        $"{nameof(AddPurchaseOrderCommand)}.Purchaser UserId",
        "For adding a Order, the Purchaser UserId must be existing.");

    public static readonly ApplicationError PurchaserItemIsEmpty = new(
        $"{nameof(AddPurchaseOrderCommand)}.Order Items Count",
        "For adding a Order, the Purchases Order goods must be specified.");
}
