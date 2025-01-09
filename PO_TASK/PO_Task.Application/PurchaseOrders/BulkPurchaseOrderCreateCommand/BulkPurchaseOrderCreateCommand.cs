using PO_Task.Application.Abstractions.Messaging;
using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Application.PurchaseOrders;


public sealed record BulkPurchaseOrderCreateCommand(
    IReadOnlyList<BulkPurchaseOrderCommand> BulkPurchaseOrderCommands
    ): ICommand<IReadOnlyList<PurchaseOrderCreateCommandResult>>;

public sealed record BulkPurchaseOrderCommand(
        PoNumberGeneratorType PONumberType,
        IEnumerable<BulkPurchaseOrderItemCreateCommand> PO_Items
    ) : ICommand<IEnumerable<Guid>>;

public sealed record BulkPurchaseOrderItemCreateCommand(
        string GoodCode,
        //int SerialNumber,
        decimal Quantity,
        decimal Price,
        string PriceCurrencyCode
    );
