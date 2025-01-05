using PO_Task.Application.Abstractions.Messaging;
using PO_Task.Domain.Users;

namespace PO_Task.Application.PurchaseOrders;

public sealed record AddPurchaseOrderCommand(
        Guid PurchaserId,
        PurchaseOrderItemCommand PurchaseOrderItem
    ) : ICommand<Guid>;

public sealed record PurchaseOrderItemCommand(
        Guid ItemId,
        string GoodCode,
        int SerialNumber,
        decimal Quantity,
        decimal Price,
        string PriceCurrencyCode
    );
