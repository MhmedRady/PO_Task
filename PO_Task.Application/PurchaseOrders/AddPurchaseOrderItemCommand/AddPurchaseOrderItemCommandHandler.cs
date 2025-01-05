using PO_Task.Application.Abstractions.Messaging;
using PO_Task.Application.Exceptions;
using PO_Task.Domain;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Common;
using PO_Task.Domain.Items;
using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Application.PurchaseOrders;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class AddPurchaseOrderItemCommandHandler(
    IPurchaseOrderRepository _purchaseOrderRepository,
    IUnitOfWork _unitOfWork) : ICommandHandler<AddPurchaseOrderItemCommand, bool>
{
    public async Task<bool> Handle(
        AddPurchaseOrderItemCommand request,
        CancellationToken cancellationToken)
    {

        var POrder = await _purchaseOrderRepository.GetByPoNumber(request.PoNumber, default)
                        ?? throw new ApplicationFlowException([AddPurchaseOrderItemCommandErrors.PurchaserNumberNotFound]);

        PurchaseOrderItem orderItem = CreateOredItem(POrder.Id, request);

        POrder.AddOrderItem(orderItem);

        return _unitOfWork.SaveChangesAsync().IsCompletedSuccessfully;
    }

    private PurchaseOrderItem CreateOredItem(PurchaseOrderId purchaseOrderId,
        AddPurchaseOrderItemCommand orderItemCommand)
    {
        var orderItem = PurchaseOrderItem.CreateInstance(
            purchaseOrderId,
            orderItemCommand.GoodCode,
            orderItemCommand.Quantity,
            new Money(
                orderItemCommand.Price,
                Currency.FromCode(orderItemCommand.PriceCurrencyCode))
            );
        return orderItem;
    }
}
