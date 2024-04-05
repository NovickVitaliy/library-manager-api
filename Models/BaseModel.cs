using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace library_manager_api.Models;

public abstract class BaseModel
{
    [BsonId]
    public ObjectId Id { get; set; }
}