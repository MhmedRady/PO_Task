using PO_Task.Application.Abstractions.Messaging;
using PO_Task.Domain.PurchaseOrders;
using PO_Task.Domain.Users;

namespace PO_Task.Application.PurchaseOrders;

public sealed record AddPurchaseOrderCommand(
        PoNumberGeneratorType PONumberType,
        IReadOnlyList<PurchaseOrderItemCommand> PurchaseOrderItems
    ) : ICommand<PurchaseOrderCreateCommandResult>;

public sealed record PurchaseOrderItemCommand(
        string GoodCode,
        decimal Quantity,
        decimal Price,
        string PriceCurrencyCode
    );
