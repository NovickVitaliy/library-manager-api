using Carter;
using library_manager_api.CQRS.Command;
using library_manager_api.DataAccess.Abstraction;
using MediatR;

namespace library_manager_api.Features.Author.DeleteAuthor;

public static class DeleteAuthor
{
    public sealed record DeleteAuthorCommand(string Id) : ICommand;
    
    public sealed class DeleteAuthorCommandHandler : ICommandHandler<DeleteAuthorCommand>
    {
        private readonly IAuthorService _authorService;

        public DeleteAuthorCommandHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            await _authorService.DeleteAuthorAsync(request.Id);

            return Unit.Value;
        }
    }
}

public sealed class DeleteAuthorModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/authors/{id}", async (string id, ISender sender) => 
            await sender.Send(new DeleteAuthor.DeleteAuthorCommand(id)));
    }
}