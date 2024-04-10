using library_manager_api.Features.Book.AddBook;
using library_manager_api.Features.Book.UpdateBook;
using library_manager_api.Features.GetAllBooks;
using library_manager_api.Models;

namespace library_manager_api.DataAccess.Abstraction;

public interface IBookService
{
    Task<string?> AddBookAsync(AddBook.AddBookCommand book);
    Task<IEnumerable<GetAllBooks.BookResponse>> GetAllBooksAsync();
    Task<GetAllBooks.BookResponse?> GetBookByIdAsync(string id);
    Task UpdateBookAsync(string id, UpdateBook.UpdateBookCommand book);
    Task DeleteBookAsync(string id);
}