using PO_Task.Domain.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace PO_Task.Domain
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder?> GetByPoNumber(string poNumber, CancellationToken cancellationToken);
        Task AddAsync(PurchaseOrder purchaseOrder);
        void Update(PurchaseOrder purchaseOrder);
        void Delete(PurchaseOrder purchaseOrder);

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
