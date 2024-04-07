using library_manager_api.Features.Author.AddAuthor;

namespace library_manager_api.DataAccess.Abstraction;

public interface IAuthorService
{
    Task<string?> AddAuthorAsync(AddAuthor.AddAuthorCommand addAuthorCommand);
}