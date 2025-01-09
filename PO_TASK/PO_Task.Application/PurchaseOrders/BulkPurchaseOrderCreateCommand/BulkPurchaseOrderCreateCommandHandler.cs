using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
public sealed class BulkPurchaseOrderCreateCommandHandler(
    PoNumberGeneratorFactory _poNumberGeneratorFactory,
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
        var createdPurchaseOrder = new List<PurchaseOrder>();
        var errors = new List<ApplicationError>();

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        int poRequestIndex = 1;

        foreach (var poRequest in request.BulkPurchaseOrderCommands)
        {
            
            try
            {
                
                PurchaseOrderId POrderId = PurchaseOrderId.CreateUnique();
                var poItems = poRequest.PO_Items.Select(itemCmd =>
                    CreateOredItem(
                        purchaseOrderId: POrderId,
                        orderItemCommand: itemCmd
                    )
                ).ToArray();

                if (!poItems.Any())
                        errors.Add(BulkPurchaseOrderCreateCommandErrors.PurchaserItemIsEmpty(poRequestIndex));

                DateTime issueDate = DateTime.Now;

                var poNumber = _poNumberGeneratorFactory.GetGenerator(poRequest.PONumberType).GeneratePoNumber(issueDate);

                var po = PurchaseOrder.CreateOrderInstance(
                    POrderId,
                    issueDate,
                    poNumber,
                    poItems
                );

                await _PoRepository.AddAsync(po);
                createdPurchaseOrderIds.Add(new(po.CreatedAt, po.PoNumber));
                ++poRequestIndex;
            }
            catch (ApplicationFlowException ex)
            {
                errors.Add(new($"{nameof(BulkPurchaseOrderCreateCommandHandler)} Error Index Number: {poRequestIndex}", ex.Message));
            }
            catch (Exception ex)
            {
                errors.Add(new($"{nameof(BulkPurchaseOrderCreateCommandHandler)} For adding Index {poRequestIndex} in a Bulk Order Create", $", the Purchaser Error: \n{ex.Message}."));
            }
        }

        if (errors.Any())
        {
            await _unitOfWork.RollbackAsync(cancellationToken);

            throw new ApplicationFlowException(
                errors.Select(e => e)
            );
        }

        try
        {
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex) 
        { 
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw new InvalidOperationException(nameof(BulkPurchaseOrderCreateCommandHandler), ex);
        }

        return createdPurchaseOrderIds;
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
