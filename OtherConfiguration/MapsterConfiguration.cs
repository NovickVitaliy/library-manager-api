using library_manager_api.Features.Author.AddAuthor;
using library_manager_api.Features.Book.AddBook;
using library_manager_api.Models;
using Mapster;

namespace library_manager_api.OtherConfiguration;

public static class MapsterConfiguration
{
    public static void ConfigureMaps()
    {
        TypeAdapterConfig<AddBook.AddBookCommand, Book>.NewConfig()
            .MapWith(src => new Book()
            {
                Title = src.Title,
                Description = src.Description,
                Language = src.Language,
                YearPublished = src.YearPublished,
                Pages = src.Pages,
                Categories = src.Categories
            });

        TypeAdapterConfig<AddAuthor.AddAuthorCommand, Author>.NewConfig()
            .MapWith(src => new Author()
            {
                FirstName = src.FirstName,
                LastName = src.LastName,
                DateOfBirth = src.DateOfBirth,
                Genres = src.Genres,
                WorkSphere = src.WorkSphere
            });
    }
}