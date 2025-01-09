using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.PurchaseOrders;

public static class PurchaseOrderStatusErrors
{
    public static readonly Error ShipmentsInPreparationToReadyForPickupRuleUserError = new(
        "Orders.ShipmentsInPreparationToReadyForPickupRule.UserError",
        "The current user does not belong to the required role to move from 'shipments in preparation' to 'shipments ready for pickup'");

    public static readonly Error ShipmentsInPreparationToReadyForPickupRuleHandoverMethodError = new(
        "Orders.ShipmentsInPreparationToReadyForPickupRule.HandoverMethod",
        "The handover method must be pickup to move from 'shipments in preparation' to 'shipments ready for pickup'");
}
