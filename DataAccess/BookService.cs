using library_manager_api.DataAccess.Abstraction;
using library_manager_api.Features.Book.AddBook;
using library_manager_api.Models;
using library_manager_api.Options;
using Mapster;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace library_manager_api.DataAccess;

public class BookService : IBookService
{
    private readonly IMongoClient _mongoClient;
    private readonly IOptionsMonitor<MongoDbSettings> _monitor;
    private readonly IMongoCollection<Book> _books;

    public BookService(IMongoClient mongoClient, IOptionsMonitor<MongoDbSettings> monitor)
    {
        _mongoClient = mongoClient;
        _monitor = monitor;
        _books = mongoClient
            .GetDatabase(monitor.CurrentValue.DatabaseName)
            .GetCollection<Book>(monitor.CurrentValue.BookCollectionName);
    }

    public async Task<string?> AddBookAsync(AddBook.AddBookCommand addBookCommand)
    {
        var book = addBookCommand.Adapt<Book>();

        book.AuthorId = new MongoDBRef(_monitor.CurrentValue.AuthorCollectionName, addBookCommand.AuthorId);
        
        await _books.InsertOneAsync(book);
        return book.Id.ToString();
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return (await _books.FindAsync<Book>(_ => true)).ToList();
    }

    public async Task<Book?> GetBookByIdAsync(string id)
    {
        return await (await _books.FindAsync(t => t.Id == new ObjectId(id))).FirstOrDefaultAsync();
    }

    public async Task UpdateBookAsync(string id, Book book)
    {
        await _books.FindOneAndReplaceAsync(t => t.Id == new ObjectId(id), book,
            new()
            {
                IsUpsert = false
            });
    }

    public async Task DeleteBookAsync(string id)
    {
        await _books.DeleteOneAsync(t => t.Id == new ObjectId(id));
    }
}