using library_manager_api.Features.Author.AddAuthor;
using library_manager_api.Features.Author.GetAllAuthors;
using library_manager_api.Features.Author.UpdateAuthor;

namespace library_manager_api.DataAccess.Abstraction;

public interface IAuthorService
{
    Task<string?> AddAuthorAsync(AddAuthor.AddAuthorCommand addAuthorCommand);
    Task<IEnumerable<GetAllAuthors.AuthorResponse>> GetAllAuthorsAsync();
    Task<GetAllAuthors.AuthorResponse?> GetAuthorByIdAsync(string id);
    Task UpdateAuthorAsync(UpdateAuthor.UpdateAuthorCommand updateAuthorCommand);
    Task DeleteAuthorAsync(string requestId);
}