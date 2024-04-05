using MediatR;

namespace library_manager_api.CQRS.Command;

public interface ICommand<out TResponse>
    : IRequest<TResponse>;

public interface ICommand
    : IRequest<Unit>;