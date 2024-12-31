using PO_Task.Domain.BuildingBlocks;
using Microsoft.EntityFrameworkCore;

namespace PO_Task.Infrastructure.Repositories;

internal abstract class Repository<T, TId> where T : Entity<TId> where TId : notnull
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(
        TId id,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<T>()
            .FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
    }

    public virtual void Add(T entity)
    {
        DbContext.Add(entity);
    }

    public virtual void Update(T entity)
    {
        DbContext.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        DbContext.Remove(entity);
    }
}
