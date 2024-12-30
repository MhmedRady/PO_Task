namespace PO_Task.Domain.BuildingBlocks;

public interface IStatusTransitionStrategy<in T, out TStatus>
{
    TStatus From { get; }
    TStatus GetNextStatus(T entity);
}
