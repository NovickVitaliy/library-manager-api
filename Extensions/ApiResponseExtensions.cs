using FluentValidation.Results;
using library_manager_api.ApiResponse;

namespace library_manager_api.Extensions;

public static class ApiResponseExtensions
{
    public static ValidationFailtureApiResponse ToValidationFailureApiResponse(this IEnumerable<ValidationFailure> failures)
    {
        var validationErrors = failures.GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage)
            .ToDictionary(
                x => x.Key, 
                x => x.ToList());
        
        var response = new ValidationFailtureApiResponse()
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Description = "Validation error occured",
            ValidationErrors = validationErrors 
        };
        
        return response;
    }
}