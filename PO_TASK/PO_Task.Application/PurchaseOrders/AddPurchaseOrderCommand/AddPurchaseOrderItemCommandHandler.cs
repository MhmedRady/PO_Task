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
    PoNumberGeneratorFactory _poNumberGeneratorFactory,
    IPurchaseOrderRepository _PoRepository,
    IUserRepository _userRepository,
    IUnitOfWork _unitOfWork) : ICommandHandler<AddPurchaseOrderCommand, PurchaseOrderCreateCommandResult>
{
    public async Task<PurchaseOrderCreateCommandResult> Handle(AddPurchaseOrderCommand request, CancellationToken cancellationToken)
    {

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {

            PurchaseOrderId purchaseOrderId = PurchaseOrderId.CreateUnique();

            var issueDate = DateTime.Now;

            var poNumber = _poNumberGeneratorFactory.GetGenerator(request.PONumberType).GeneratePoNumber(issueDate);

            var poItems = request.PurchaseOrderItems.Select(poItem => CreateOredItem(purchaseOrderId, poItem)).ToArray();

            if (!poItems.Any())
                throw new ApplicationFlowException([AddPurchaseOrderCommandErrors.PurchaserItemIsEmpty]);

            var purchaseOrder = PurchaseOrder.CreateOrderInstance(
                    purchaseOrderId,
                    issueDate,
                    poNumber,
                    poItems
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
