using MediatR;

namespace PO_Task.Application.Abstractions.Messaging;

public interface IBaseCommand
{
}

public interface ICommand : IRequest, IBaseCommand
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>, IBaseCommand
{
}
