using MediatR;

namespace library_manager_api.CQRS.Query;

public interface IQuery<out TResponse>
    : IRequest<TResponse>;