using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace library_manager_api.Models;

public class Book : BaseModel
{
    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Language { get; set; } = default!;

    public ushort YearPublished { get; set; } = default!;

    public ICollection<string> Categories { get; set; } = default!;

    public ushort Pages { get; set; } = default!;
}