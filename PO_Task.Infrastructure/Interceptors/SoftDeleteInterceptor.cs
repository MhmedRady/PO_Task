using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Infrastructure.Interceptors;

internal sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return ValueTask.FromResult(result);
        }

        foreach (EntityEntry<ISoftDelete> entry in eventData.Context.ChangeTracker.Entries<ISoftDelete>())
        {
            if (entry is not { State: EntityState.Deleted })
            {
                continue;
            }

            entry.State = EntityState.Modified;
            entry.Entity.MarkAsDeleted();
        }

        return ValueTask.FromResult(result);
    }
}
