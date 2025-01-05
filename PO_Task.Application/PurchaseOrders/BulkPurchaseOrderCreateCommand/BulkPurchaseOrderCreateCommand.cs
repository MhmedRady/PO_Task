using PO_Task.Application.Abstractions.Messaging;
using PO_Task.Domain.Users;

namespace PO_Task.Application.PurchaseOrders;

public sealed record BulkPurchaseOrderCreateCommand(
        Guid PurchaserId,
        DateTime IssueDate,
        IEnumerable<BulkOrderItemsCreateCommand> PurchaseOrderItems
    ) : ICommand<IEnumerable<Guid>>;

public sealed record BulkOrderItemsCreateCommand(
        Guid ItemId,
        string GoodCode,
        int SerialNumber,
        decimal Quantity,
        decimal Price,
        string PriceCurrencyCode
    );
