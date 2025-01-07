using PO_Task.Application.Abstractions.Messaging;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Users;
using PO_Task.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PO_Task.Domain.Items;
using PO_Task.Domain.PurchaseOrders;
using PO_Task.Application.Exceptions;
using PO_Task.Domain.Common;

namespace PO_Task.Application.PurchaseOrders;

internal class AddPurchaseOrderCommandHandler(
    IPurchaseOrderRepository _PoRepository,
    IUserRepository _userRepository,
    IUnitOfWork _unitOfWork) : ICommandHandler<AddPurchaseOrderCommand, PurchaseOrderCreateCommandResult>
{
    public async Task<PurchaseOrderCreateCommandResult> Handle(AddPurchaseOrderCommand request, CancellationToken cancellationToken)
    {

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var isExistUserId = _userRepository.GetByIdAsync(UserId.Create(request.PurchaserId))
                    ?? throw new ApplicationFlowException([AddPurchaseOrderCommandErrors.PurchaserIdNotFound]);

            PurchaseOrderId purchaseOrderId = PurchaseOrderId.CreateUnique();

            var purchaseOrder = PurchaseOrder.CreateOrderInstance(
                    purchaseOrderId,
                    UserId.Create(request.PurchaserId),
                    DateTime.Now,
                    request.PurchaseOrderItems.Select(poItem => CreateOredItem(purchaseOrderId, poItem)).ToArray()
                );

            await _PoRepository.AddAsync(purchaseOrder);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new PurchaseOrderCreateCommandResult(purchaseOrder.CreatedAt, purchaseOrder.PoNumber);
        }
        catch (Exception ex)
        {
            throw new ApplicationFlowException([new( nameof(AddPurchaseOrderCommandHandler), ex.Message)]);
        }
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
