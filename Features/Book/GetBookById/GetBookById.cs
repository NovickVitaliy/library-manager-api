using Carter;
using library_manager_api.CQRS.Query;
using library_manager_api.DataAccess.Abstraction;
using MediatR;

namespace library_manager_api.Features.Book.GetBookById;

public static class GetBookById
{
    public sealed record GetBookByIdQuery(string Id)
        : IQuery<GetAllBooks.GetAllBooks.BookResponse?>;

    public sealed class GetBookByIdQueryHandler
        : IQueryHandler<GetBookByIdQuery, GetAllBooks.GetAllBooks.BookResponse?>
    {
        private readonly IBookService _bookService;

        public GetBookByIdQueryHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<GetAllBooks.GetAllBooks.BookResponse?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetBookByIdAsync(request.Id);
        }
    }
}

public sealed class GetBookByIdModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/books/{id}", async (string id, ISender sender) =>
        {
            var book = await sender.Send(new GetBookById.GetBookByIdQuery(id));

            if (book is null)
                return Results.NotFound();
            
            return Results.Json(book);
        });
    }
}