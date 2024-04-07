using library_manager_api.DataAccess.Abstraction;
using library_manager_api.Features.Author.AddAuthor;
using library_manager_api.Features.Author.GetAllAuthors;
using library_manager_api.Models;
using library_manager_api.Options;
using Mapster;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace library_manager_api.DataAccess;

public class AuthorService : IAuthorService
{
    private readonly IMongoClient _mongoClient;
    private readonly IOptionsMonitor<MongoDbSettings> _monitor;
    private readonly IMongoCollection<Author> _authors;

    public AuthorService(IMongoClient mongoClient, IOptionsMonitor<MongoDbSettings> monitor)
    {
        _mongoClient = mongoClient;
        _monitor = monitor;
        _authors = mongoClient.GetDatabase(monitor.CurrentValue.DatabaseName).GetCollection<Author>(monitor.CurrentValue.AuthorCollectionName);
    }

    public async Task<string?> AddAuthorAsync(AddAuthor.AddAuthorCommand addAuthorCommand)
    {
        var author = addAuthorCommand.Adapt<Author>();

        await _authors.InsertOneAsync(author);

        return author.Id.ToString();
    }

    public async Task<IEnumerable<GetAllAuthors.AuthorResponse>> GetAllAuthorsAsync()
    {
        return (await (await _authors.FindAsync("{}")).ToListAsync())
            .Select(a => a.Adapt<GetAllAuthors.AuthorResponse>());
    }
}