using PO_Task.Application.Abstractions.Messaging;

namespace PO_Task.Application.PurchaseOrders;

public sealed record AddPurchaseOrderItemCommand(
    string PoNumber,
    string GoodCode,
    //int SerialNumber,
    decimal Quantity,
    decimal Price,
    string PriceCurrencyCode
    ) : ICommand<bool>;