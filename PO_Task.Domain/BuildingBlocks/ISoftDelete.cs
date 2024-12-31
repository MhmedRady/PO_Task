namespace PO_Task.Domain.BuildingBlocks;

public interface ISoftDelete
{
    public DateTimeOffset? DeletedAt { get; }

    public void MarkAsDeleted();
}
