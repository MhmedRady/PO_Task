using PO_Task.Domain;
using PO_Task.Domain.Items;

namespace PO_Task.Infrastructure.Repositories;

internal sealed class OrderItemRepository :
    Repository<PurchaseOrderItem, ItemId>, IOrderItemRepository
{
    public OrderItemRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}
