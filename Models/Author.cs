using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace library_manager_api.Models;

public class Author : BaseModel
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public DateOnly DateOfBirth { get; set; } = default!;
}