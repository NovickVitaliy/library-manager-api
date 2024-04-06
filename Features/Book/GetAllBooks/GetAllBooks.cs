using Carter;
using library_manager_api.CQRS.Query;
using library_manager_api.DataAccess.Abstraction;
using Mapster;
using MediatR;

namespace library_manager_api.Features.GetAllBooks;

public static class GetAllBooks
{
    public sealed record BookResponse(
        string Id,
        string Title,
        string Description,
        string Language,
        short YearPublished,
        IEnumerable<string> Categories,
        short Pages);

    public sealed record GetAllBooksQuery : IQuery<IEnumerable<BookResponse>>;
    
    public sealed class GellAllBooksQueryHandler : IQueryHandler<GetAllBooksQuery, IEnumerable<BookResponse>>
    {
        private readonly IBookService _bookService;

        public GellAllBooksQueryHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IEnumerable<BookResponse>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            return (await _bookService.GetAllBooksAsync()).Select(b => b.Adapt<BookResponse>());
        }
    }
}

public sealed class GetAllBooksModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/books", async (ISender sender) => 
            Results.Json(await sender.Send(new GetAllBooks.GetAllBooksQuery())));
    }
}