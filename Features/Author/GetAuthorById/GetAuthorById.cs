using Carter;
using library_manager_api.CQRS.Query;
using library_manager_api.DataAccess.Abstraction;
using MediatR;

namespace library_manager_api.Features.Author.GetAuthorById;

public static class GetAuthorById
{
    public sealed record GetAuthorByIdQuery(string Id) : IQuery<GetAllAuthors.GetAllAuthors.AuthorResponse?>;
    
    public sealed class GetAuthorByIdQueryHandler : IQueryHandler<GetAuthorByIdQuery, GetAllAuthors.GetAllAuthors.AuthorResponse?>
    {
        private readonly IAuthorService _authorService;

        public GetAuthorByIdQueryHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public Task<GetAllAuthors.GetAllAuthors.AuthorResponse?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            return _authorService.GetAuthorByIdAsync(request.Id);
        }
    }
}

public sealed class GetAuthorByIdModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/authors/{id}", async (string id, ISender sender) => 
            await sender.Send(new GetAuthorById.GetAuthorByIdQuery(id)));
    }
}