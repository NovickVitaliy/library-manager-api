using library_manager_api.ApiResponse;

namespace library_manager_api.Exceptions;

public abstract class BaseException : Exception
{
    protected BaseException()
    {
    }

    protected BaseException(string? message) : base(message)
    {
    }

    public abstract BaseApiResponse ToApiResponse();
    
    
}