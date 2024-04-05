using Carter;
using library_manager_api.CQRS.Command;
using library_manager_api.DataAccess.Abstraction;
using library_manager_api.Models;
using Mapster;
using MediatR;

namespace library_manager_api.Features.AddBook;

public static class AddBook
{
    public sealed record AddBookRequest(
        string Title,
        string Description,
        string Language,
        ushort YearPublished,
        ICollection<string> Categories,
        ushort Pages);

    public sealed record AddBookCommand(
        string Title,
        string Description,
        string Language,
        ushort YearPublished,
        ICollection<string> Categories,
        ushort Pages) : ICommand<string>;
    
    public sealed class AddBookCommandHandler : ICommandHandler<AddBookCommand, string>
    {
        private readonly IBookService _bookService;

        public AddBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<string> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            //validation

            var book = request.Adapt<Book>();

            return await _bookService.AddBookAsync(book);
        }
    }
    
    public sealed class AddBookModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/books", async (AddBookRequest addBookRequest, ISender sender) =>
            {
                var cmd = addBookRequest.Adapt<AddBookCommand>();

                var bookId = await sender.Send(cmd);

                return Results.Created($"/products/{bookId}", bookId);
            });
        }
    }
}