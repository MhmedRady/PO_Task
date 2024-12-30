namespace PO_Task.Domain.BuildingBlocks;

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IHasEvents
#pragma warning restore S4035
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected Entity(TId id)
    {
        if (!IsValid(id))
        {
            throw new ArgumentException("Identifier is not in a supported format");
        }

        Id = id;
    }

    protected Entity()
    {
    }

    public TId Id { get; protected set; }

    public bool Equals(Entity<TId>? other)
    {
        return Equals((object?)other);
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    public static bool operator ==(
        Entity<TId> left,
        Entity<TId> right)
    {
        return Equals(
            left,
            right);
    }

    public static bool operator !=(
        Entity<TId> left,
        Entity<TId> right)
    {
        return !Equals(
            left,
            right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    private bool IsValid(TId id)
    {
        return id is int || id is long || id is ValueObject || id is Guid;
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
