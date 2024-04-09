using Carter;
using FluentValidation;
using library_manager_api.CQRS.Command;
using library_manager_api.DataAccess.Abstraction;
using library_manager_api.Exceptions;
using Mapster;
using MediatR;
using ValidationException = library_manager_api.Exceptions.ValidationException;

namespace library_manager_api.Features.Author.UpdateAuthor;

public static class UpdateAuthor
{
    public sealed record UpdateAuthorRequest(
            string FirstName,
            string LastName,
            DateOnly DateOfBirth,
            string WorkSphere,
            string[] Genres
        );

    public sealed record UpdateAuthorCommand(
            string Id,
            string FirstName,
            string LastName,
            DateOnly DateOfBirth,
            string WorkSphere,
            string[] Genres
        ) : ICommand;
    
    public sealed class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("{PropertyName} must be supplied");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("{PropertyName} must be supplied");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("{PropertyName} must be supplied")
                .GreaterThan(DateOnly.MinValue).WithMessage("{PropertyName} cannot be less than {ComparisonValue}");

            RuleFor(x => x.WorkSphere)
                .NotEmpty().WithMessage("{PropertyName} must be supplied");

            RuleFor(x => x.Genres)
                .NotEmpty().WithMessage("{PropertyName} must be supplied");
        }
    }
    
    
    public sealed class UpdateAuthorCommandHandler : ICommandHandler<UpdateAuthorCommand>
    {
        private readonly IAuthorService _authorService;

        public UpdateAuthorCommandHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public async Task<Unit> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            await _authorService.UpdateAuthorAsync(request);

            return Unit.Value;
        }
    }
}

public sealed class UpdateAuthorModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/authors/{id}", async (string id, UpdateAuthor.UpdateAuthorRequest request, ISender sender) =>
        {
            var validator = new UpdateAuthor.UpdateAuthorRequestValidator();
            var result = await validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            await sender.Send(new UpdateAuthor.UpdateAuthorCommand(
                id,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.WorkSphere,
                request.Genres
                ));

            return Results.NoContent();
        });
    }
}