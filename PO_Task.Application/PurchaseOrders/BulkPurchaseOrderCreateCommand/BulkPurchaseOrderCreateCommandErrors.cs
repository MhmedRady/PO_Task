using PO_Task.Application.Exceptions;
using PO_Task.Domain.BuildingBlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO_Task.Application.PurchaseOrders;

public static class BulkPurchaseOrderCreateCommandErrors
{
    public static readonly ApplicationError PurchaserIdNotFound = new(
        $"{nameof(BulkPurchaseOrderCreateCommand)}.Purchaser UserId",
        "For adding a Order, the Purchaser UserId must be existing.");

    public static readonly ApplicationError NotFound = new(
        $"{nameof(BulkPurchaseOrderCreateCommand)}.Purchaser UserId",
        "For adding a Order, the Purchaser UserId must be existing.");
}
