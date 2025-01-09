using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO_Task.Application.PurchaseOrders
{
    public sealed record PurchaseOrderCreateCommandResult(DateTime IssueDate, string PONumber);
}
