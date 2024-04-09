using library_manager_api.ApiResponse;

namespace library_manager_api.Exceptions.Author;

public class AuthorNotFoundException : BaseException
{
    private readonly string _authorId;

    public AuthorNotFoundException(string authorId)
        : base($"Authow with ID:{authorId} was not found")
    {
        _authorId = authorId;
    }

    public override BaseApiResponse ToApiResponse()
    {
        return new NotFoundApiFailure()
        {
            Id = _authorId,
            StatusCode = StatusCodes.Status404NotFound,
            Description = $"Author with Id of {_authorId} was not found"
        };
    }
}