using library_manager_api.Features.Book.AddBook;
using library_manager_api.Models;

namespace library_manager_api.DataAccess.Abstraction;

public interface IBookService
{
    Task<string?> AddBookAsync(AddBook.AddBookCommand book);
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book?> GetBookByIdAsync(string id);
    Task UpdateBookAsync(string id, Book book);
    Task DeleteBookAsync(string id);
}