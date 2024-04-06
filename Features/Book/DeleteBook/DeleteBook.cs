using Carter;
using library_manager_api.CQRS.Command;
using library_manager_api.DataAccess.Abstraction;
using MediatR;

namespace library_manager_api.Features.Book.DeleteBook;

public static class DeleteBook
{
    public sealed record DeleteBookCommand(string Id)
        : ICommand;

    public sealed class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand>
    {
        private readonly IBookService _bookService;

        public DeleteBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            await _bookService.DeleteBookAsync(request.Id);

            return Unit.Value;
        }
    }
}

public sealed class DeleteBookModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/books/{id}", async (string id, ISender sender) =>
        {
            await sender.Send(new DeleteBook.DeleteBookCommand(id));

            return Results.NoContent();
        });
    }
}