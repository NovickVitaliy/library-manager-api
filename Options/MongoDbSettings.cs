using System.ComponentModel.DataAnnotations;

namespace library_manager_api.Options;

public record MongoDbSettings(
    [Required] string ConnectionString,
    [Required] string DatabaseName,
    [Required] string BookCollectionName,
    [Required] string AuthorCollectionName)
{
    public const string Position = "MongoDBSettings";
};