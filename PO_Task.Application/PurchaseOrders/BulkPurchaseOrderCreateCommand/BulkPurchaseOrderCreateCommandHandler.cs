using PO_Task.Application.Abstractions.Messaging;
using PO_Task.Application.Exceptions;
using PO_Task.Domain;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Common;
using PO_Task.Domain.Items;
using PO_Task.Domain.PurchaseOrders;
using PO_Task.Domain.Users;
using System.Net.Http.Headers;

namespace PO_Task.Application.PurchaseOrders;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class BulkPurchaseOrderCreateCommandHandler(
    IPurchaseOrderRepository _PoRepository,
    IUserRepository _userRepository,
    IUnitOfWork _unitOfWork) : ICommandHandler<BulkPurchaseOrderCreateCommand,
        IReadOnlyList<PurchaseOrderCreateCommandResult>>
{
    public async Task<IReadOnlyList<PurchaseOrderCreateCommandResult>> Handle(
        BulkPurchaseOrderCreateCommand request,
        CancellationToken cancellationToken)
    {

        var createdPurchaseOrderIds = new List<PurchaseOrderCreateCommandResult>();
        var errors = new List<ApplicationError>();

        using var poTransaction = await _PoRepository.BeginTransactionAsync();

        foreach (var poRequest in request.BulkPurchaseOrderCommands)
        {
            try
            {
                var isExistUserId = await _userRepository.GetByIdAsync(UserId.Create(poRequest.PurchaserId));

                if (isExistUserId is not User)
                {
                    errors.Add(BulkPurchaseOrderCreateCommandErrors.PurchaserIdNotFound);
                    continue;
                }


                PurchaseOrderId POrderId = PurchaseOrderId.CreateUnique();
                var poItems = poRequest.PO_Items.Select(itemCmd =>
                    CreateOredItem(
                        purchaseOrderId: POrderId,
                        orderItemCommand: itemCmd
                    )
                ).ToArray();

                var po = PurchaseOrder.CreateOrderInstance(
                    POrderId,
                    isExistUserId.Id,
                    poRequest.IssueDate,
                    poItems
                );

                await _PoRepository.AddAsync(po);
                createdPurchaseOrderIds.Add(new(po.CreatedAt, po.PoNumber));
            }
            catch (ApplicationFlowException ex)
            {
                errors.Add(new ApplicationError($"{nameof(AddPurchaseOrderCommand)}.Purchaser Bulk Create", ex.Message));
            }
            catch (Exception ex)
            {
                errors.Add(new ApplicationError(
                            $"{nameof(AddPurchaseOrderCommand)}.Purchaser Bulk Create",
                            $"For adding a Bulk Order Create , the Purchaser Error: {ex.Message}."));
            }
        }

        if (errors.Any())
        {
            await poTransaction.RollbackAsync(cancellationToken);

            throw new ApplicationFlowException(
                errors.Select(e => e)
            );
        }

        try
        {
            await poTransaction.CommitAsync(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }catch(Exception ex) { }

        return createdPurchaseOrderIds;

        /*        var isExistUserId = await _userRepository.GetByIdAsync(UserId.Create(request.PurchaserId))
                        ?? throw new ValidationException([AddPurchaseOrderCommandErrors.PurchaserIdNotFound]);

                PurchaseOrderId purchaseOrderId = PurchaseOrderId.CreateUnique();

                var purchaseOrder = PurchaseOrder.CreateOrderInstance(
                        purchaseOrderId,
                        UserId.Create(request.PurchaserId)
                    );

                PurchaseOrderItem orderItem = CreateOredItem(
                    purchaseOrderId,
                    request.PurchaseOrderItem);

                purchaseOrder.AddOrderItem(orderItem);

                _purchaseOrderRepository.Add(purchaseOrder);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return purchaseOrder.Id.Value;*/
    }

    private PurchaseOrderItem CreateOredItem(PurchaseOrderId purchaseOrderId,
        BulkPurchaseOrderItemCreateCommand orderItemCommand)
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
