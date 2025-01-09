using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using PO_Task.Application.Abstractions;
using PO_Task.Application.Exceptions;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Items;
using PO_Task.Domain.PurchaseOrders;
using PO_Task.Domain.Users;
using PO_Task.Infrastructure.Outbox;
using System.Linq;

namespace PO_Task.Infrastructure;

public sealed class ApplicationDbContext: DbContext, IUnitOfWork
{
    private IDbContextTransaction _currentTransaction;
    private readonly IDateTimeProvider _dateTimeProvider;
    public ApplicationDbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public DbSet<User> user_profile { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            AddDomainEventsAsOutboxMessages();

            int result = await base.SaveChangesAsync(cancellationToken);
            
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    private void AddDomainEventsAsOutboxMessages()
    {
        var outboxMessages = ChangeTracker
            .Entries<IHasEvents>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyList<IDomainEvent> domainEvents = entity.GetDomainEvents();
                
                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                _dateTimeProvider.UtcNow,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)))
            .ToList();

        AddRange(outboxMessages);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if(_currentTransaction != null)
            return;
        //base.ChangeTracker.Clear();
        _currentTransaction = await base.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("No transaction started.");

        // Save changes
        //ChangeTracker.Clear()
        try
        {
            await SaveChangesAsync();
            await _currentTransaction.CommitAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
        finally
        {
            
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction == null)
            return;

        await _currentTransaction.RollbackAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }
}
