using Carter;
using library_manager_api.CQRS.Query;
using library_manager_api.DataAccess.Abstraction;
using MediatR;

namespace library_manager_api.Features.Author.GetAllAuthors;

public static class GetAllAuthors
{
    public sealed record AuthorResponse(
        string Id,
        string FirstName,
        string LastName,
        DateOnly DateOfBirth,
        string WorkSphere,
        string[] Genres
        );

    public sealed record GetAllAuthorsQuery() : IQuery<IEnumerable<AuthorResponse>>;

    public sealed class GetAllAuthorsQueryHandler : IQueryHandler<GetAllAuthorsQuery, IEnumerable<AuthorResponse>>
    {
        private readonly IAuthorService _authorService;

        public GetAllAuthorsQueryHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public Task<IEnumerable<AuthorResponse>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            return _authorService.GetAllAuthorsAsync();
        }
    }
}

public sealed class GetAllAuthorsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/authors", async (ISender sender) =>
        {
            var authors = await sender.Send(new GetAllAuthors.GetAllAuthorsQuery());
           
            return Results.Json(authors);
        });
    }
}