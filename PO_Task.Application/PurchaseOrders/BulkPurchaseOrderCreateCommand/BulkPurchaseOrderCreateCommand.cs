using PO_Task.Application.Abstractions.Messaging;

namespace PO_Task.Application.PurchaseOrders;


public sealed record BulkPurchaseOrderCreateCommand(
    IReadOnlyList<BulkPurchaseOrderCommand> BulkPurchaseOrderCommands
    ): ICommand<IReadOnlyList<PurchaseOrderCreateCommandResult>>;

public sealed record BulkPurchaseOrderCommand(
        Guid PurchaserId,
        DateTime IssueDate,
        IEnumerable<BulkPurchaseOrderItemCreateCommand> PO_Items
    ) : ICommand<IEnumerable<Guid>>;

public sealed record BulkPurchaseOrderItemCreateCommand(
        string GoodCode,
        //int SerialNumber,
        decimal Quantity,
        decimal Price,
        string PriceCurrencyCode
    );
