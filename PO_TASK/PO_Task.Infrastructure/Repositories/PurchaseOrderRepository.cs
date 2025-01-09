using Microsoft.EntityFrameworkCore;
using PO_Task.Domain;
using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Infrastructure.Repositories;

internal sealed class PurchaseOrderRepository :
    Repository<PurchaseOrder, PurchaseOrderId>, IPurchaseOrderRepository
{
    public PurchaseOrderRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<PurchaseOrder?> GetByPoNumber(string poNumber, CancellationToken cancellationToken)
    {
        return await GetBy(po => string.Equals(poNumber, po.PoNumber, StringComparison.OrdinalIgnoreCase)).Include(p => p.PurchaseOrderItems).FirstOrDefaultAsync(cancellationToken);
    }


}
