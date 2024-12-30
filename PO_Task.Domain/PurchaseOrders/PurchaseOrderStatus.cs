using Ardalis.SmartEnum;
using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.PurchaseOrders;

public sealed class PurchaseOrderStatus : SmartEnum<PurchaseOrderStatus>, IStatusTransition<PurchaseOrderStatus>
{
    private PurchaseOrderStatus(
        string name,
        int value) : base(
        name,
        value)
    {
    }

    public static readonly PurchaseOrderStatus Ordered = new(
        nameof(Ordered),
        100,
        ["Registered"]);

    public static readonly PurchaseOrderStatus Created = new(
        nameof(Created),
        110,
        ["Registered"]);

    public static readonly PurchaseOrderStatus Approved = new(
        nameof(Approved),
        120,
        ["Registered"]);

    public static readonly PurchaseOrderStatus ShipmentsInPreparation = new(
        nameof(ShipmentsInPreparation),
        200,
        ["Registered"]);

    public static readonly PurchaseOrderStatus ShipmentsPartiallyPrepared = new(
        nameof(ShipmentsPartiallyPrepared),
        210,
        ["Registered"]);

    public static readonly PurchaseOrderStatus ShipmentsIsShipped = new(
        nameof(ShipmentsIsShipped),
        220,
        ["Registered"]);

    public static readonly PurchaseOrderStatus Completed = new(
        nameof(Completed),
        300,
        ["Registered"]);

    private static readonly Dictionary<PurchaseOrderStatus, List<PurchaseOrderStatus>> AllowedTransitions = new()
    {
        {
            Ordered, [
                Created,
                Approved
            ]
        },
        {
            ShipmentsInPreparation, [
                ShipmentsPartiallyPrepared,
                ShipmentsIsShipped,
            ]
        },
        { Completed, [] }
    };

    private PurchaseOrderStatus(
        string name,
        int value,
        List<string> roleNames) : base(
        name,
        value)
    {
        RoleNames = roleNames;
    }

    public List<string> RoleNames { get; }

    public bool CanTransitionTo(PurchaseOrderStatus newStatus)
    {
        return AllowedTransitions[this].Contains(newStatus);
    }

    public static IReadOnlyList<PurchaseOrderStatus> All()
    {
        return new List<PurchaseOrderStatus>
        {
            Ordered,
            Created,
            Approved,

            ShipmentsInPreparation,
            ShipmentsPartiallyPrepared,
            ShipmentsIsShipped,

            Completed
        };
    }
}
