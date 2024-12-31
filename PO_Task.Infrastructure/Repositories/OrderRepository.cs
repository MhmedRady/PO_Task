using PO_Task.Domain;
using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Infrastructure.Repositories;

internal sealed class OrderRepository :
    Repository<PurchaseOrder, PurchaseOrderId>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}
