using FluentValidation.Results;
using library_manager_api.ApiResponse;
using library_manager_api.Extensions;

namespace library_manager_api.Exceptions;

public class ValidationException : BaseException
{
    private readonly IEnumerable<ValidationFailure> _validationErrors;

    public ValidationException(IEnumerable<ValidationFailure> validationErrors) 
        : base("Validation error occured")
    {
        _validationErrors = validationErrors;
    }
    public override BaseApiResponse ToApiResponse()
    {
        return _validationErrors.ToValidationFailureApiResponse();
    }
}