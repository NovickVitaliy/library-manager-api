namespace library_manager_api.Exceptions.Author;

public class AuthorNotFoundException : Exception
{
    public AuthorNotFoundException(string authorId)
        : base($"Authow with ID:{authorId} was not found")
    {
    }
}