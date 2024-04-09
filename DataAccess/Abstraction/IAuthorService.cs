using library_manager_api.Features.Author.AddAuthor;
using library_manager_api.Features.Author.GetAllAuthors;

namespace library_manager_api.DataAccess.Abstraction;

public interface IAuthorService
{
    Task<string?> AddAuthorAsync(AddAuthor.AddAuthorCommand addAuthorCommand);
    Task<IEnumerable<GetAllAuthors.AuthorResponse>> GetAllAuthorsAsync();
    Task<GetAllAuthors.AuthorResponse?> GetAuthorByIdAsync(string id);
}