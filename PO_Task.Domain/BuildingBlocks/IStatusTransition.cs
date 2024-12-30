using Ardalis.SmartEnum;

namespace PO_Task.Domain.BuildingBlocks;

public interface IStatusTransition<TStatus> where TStatus : SmartEnum<TStatus>
{
    List<string> RoleNames { get; }
    bool CanTransitionTo(TStatus newStatus);
}
