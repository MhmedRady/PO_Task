using PO_Task.Domain.BuildingBlocks;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

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
        return await GetBy(entity => entity.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);
    }

    public IQueryable<T> GetBy(Expression<Func<T, bool>> expression)
    {
        return DbContext
        .Set<T>().Where(expression);
    }

    public virtual async Task AddAsync(T entity)
    {
       //EntityStateDetached(entity);
       await DbContext.AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
        DbContext.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        DbContext.Remove(entity);
    }

    private void EntityStateDetached(T Entity)
    {
        DbContext.Entry(Entity).State = EntityState.Detached;
    }

}
