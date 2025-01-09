using PO_Task.Application.Exceptions;

namespace PO_Task.Application.PurchaseOrders;

public static class AddPurchaseOrderItemCommandErrors
{
    public static readonly ApplicationError PurchaserNumberNotFound = new(
        $"{nameof(AddPurchaseOrderItemCommand)}.Purchaser Order Number",
        "For adding a Order Item, the Purchaser Order is Not exist.");
}
