using PO_Task.Application.Abstractions.Messaging;
using PO_Task.Application.Exceptions;
using PO_Task.Domain;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Common;
using PO_Task.Domain.Items;
using PO_Task.Domain.PurchaseOrders;
using PO_Task.Domain.Users;
using System.Linq;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PO_Task.Application.PurchaseOrders;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class BulkPurchaseOrderCreateCommandHandler(
    IPurchaseOrderRepository _PoRepository,
    IUserRepository _userRepository,
    IUnitOfWork _unitOfWork) : ICommandHandler<IEnumerable<BulkPurchaseOrderCreateCommand>, IEnumerable<Guid>>
{
    public async Task<IEnumerable<Guid>> Handle(
        IEnumerable<BulkPurchaseOrderCreateCommand> request,
        CancellationToken cancellationToken)
    {

        var createdPurchaseOrderIds = new List<PurchaseOrder>();
        var errors = new List<ApplicationError>();

        using var poTransaction = await _PoRepository.BeginTransactionAsync();

        foreach (var poRequest in request)
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
                var poItems = poRequest.PurchaseOrderItems.Select(itemCmd =>
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
                createdPurchaseOrderIds.Add(po);
            }
            catch(ApplicationFlowException ex)
            {
                errors.Add(new ApplicationError($"{nameof(AddPurchaseOrderCommand)}.Purchaser Bulk Create", ex.Message);
            }
            catch (Exception ex)
            {
                errors.Add(new ApplicationError(
                            $"{nameof(AddPurchaseOrderCommand)}.Purchaser Bulk Create",
                            $"For adding a Bulk Order Create , the Purchaser Error: {ex.Message}."));
            }

            if (errors.Any())
            {
                await poTransaction.RollbackAsync(cancellationToken);

                throw new ApplicationFlowException(
                    errors.Select(e => e)
                );
            }


            await poTransaction.CommitAsync(cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return createdPurchaseOrderIds.Select(po=>po.Id.Value);
        }

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
        BulkOrderItemsCreateCommand orderItemCommand)
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
