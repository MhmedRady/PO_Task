using PO_Task.Application.Abstractions.Messaging;
using PO_Task.Domain.Users;

namespace PO_Task.Application.PurchaseOrders;

public sealed record AddPurchaseOrderCommand(
        Guid PurchaserId,
        IReadOnlyList<PurchaseOrderItemCommand> PurchaseOrderItems
    ) : ICommand<PurchaseOrderCreateCommandResult>;

public sealed record PurchaseOrderItemCommand(
        string GoodCode,
        decimal Quantity,
        decimal Price,
        string PriceCurrencyCode
    );
