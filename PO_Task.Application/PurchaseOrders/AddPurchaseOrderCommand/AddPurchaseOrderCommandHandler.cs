using PO_Task.Application.Abstractions.Messaging;
using PO_Task.Application.Exceptions;
using PO_Task.Domain;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Common;
using PO_Task.Domain.Items;
using PO_Task.Domain.PurchaseOrders;
using PO_Task.Domain.Users;

namespace PO_Task.Application.PurchaseOrders;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class AddPurchaseOrderItemCommandHandler(
    IPurchaseOrderRepository _purchaseOrderRepository,
    IUserRepository _userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddPurchaseOrderCommand, Guid>
{
    public async Task<Guid> Handle(
        AddPurchaseOrderCommand request,
        CancellationToken cancellationToken)
    {

        var isExistUserId = await _userRepository.GetByIdAsync(UserId.Create(request.PurchaserId))
                ?? throw new ValidationException([AddPurchaseOrderCommandErrors.PurchaserIdNotFound]);

        PurchaseOrderId purchaseOrderId = PurchaseOrderId.CreateUnique();

        var purchaseOrder = PurchaseOrder.CreateOrderInstance(
                purchaseOrderId,
                UserId.Create(request.PurchaserId),
                DateTime.Now
            );

        PurchaseOrderItem orderItem = CreateOredItem(
            purchaseOrderId,
            request.PurchaseOrderItem);

        purchaseOrder.AddOrderItem(orderItem);

        _purchaseOrderRepository.Add(purchaseOrder);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return purchaseOrder.Id.Value;
    }

    private PurchaseOrderItem CreateOredItem(PurchaseOrderId purchaseOrderId,
        PurchaseOrderItemCommand orderItemCommand)
    {
        var orderItem = PurchaseOrderItem.CreateInstance(
            purchaseOrderId,
            orderItemCommand.GoodCode,
            //orderItemCommand.SerialNumber,
            orderItemCommand.Quantity,
            new Money(
                orderItemCommand.Price * orderItemCommand.Quantity,
                Currency.FromCode(orderItemCommand.PriceCurrencyCode))
            );
        return orderItem;
    }
}
