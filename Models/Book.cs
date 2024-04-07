using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace library_manager_api.Models;

public class Book : BaseModel
{
    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Language { get; set; } = default!;

    public short YearPublished { get; set; } = default!;

    public ICollection<string> Categories { get; set; } = default!;

    public short Pages { get; set; } = default!;
    
    public MongoDBRef AuthorId { get; set; } = default!;
}