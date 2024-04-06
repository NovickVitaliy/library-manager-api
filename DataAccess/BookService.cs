using library_manager_api.DataAccess.Abstraction;
using library_manager_api.Models;
using library_manager_api.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace library_manager_api.DataAccess;

public class BookService : IBookService
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoCollection<Book> _books;
    
    public BookService(IMongoClient mongoClient, IOptionsMonitor<MongoDbSettings> monitor)
    {
        _mongoClient = mongoClient;
        _books = mongoClient
            .GetDatabase(monitor.CurrentValue.DatabaseName)
            .GetCollection<Book>(monitor.CurrentValue.BookCollectionName);
    }

    public async Task<string> AddBookAsync(Book book)
    {
        await _books.InsertOneAsync(book);
        return book.Id.ToString();
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return (await _books.FindAsync<Book>(_ => true)).ToList();
    }
}