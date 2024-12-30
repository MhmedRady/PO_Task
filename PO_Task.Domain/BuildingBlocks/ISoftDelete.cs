namespace PO_Task.Domain.BuildingBlocks;

public interface ISoftDelete
{
    public bool IsDeleted { get; }
    public DateTimeOffset? DeletedAt { get; }

    public void MarkAsDeleted();
}
