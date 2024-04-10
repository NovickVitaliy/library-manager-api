using Carter;
using FluentValidation;
using library_manager_api.CQRS.Command;
using library_manager_api.DataAccess.Abstraction;
using library_manager_api.Extensions;
using Mapster;
using MediatR;

namespace library_manager_api.Features.Author.AddAuthor;

public static class AddAuthor
{
    public sealed record AddAuthorRequest(
        string FirstName,
        string LastName,
        DateOnly DateOfBirth,
        string WorkSphere,
        string[] Genres
    );

    public sealed record AddAuthorCommand(
        string FirstName,
        string LastName,
        DateOnly DateOfBirth,
        string WorkSphere,
        string[] Genres
    ) : ICommand<string>;
    
    public sealed class AddAuthorCommandHandler : ICommandHandler<AddAuthorCommand, string>
    {
        private readonly IAuthorService _authorService;

        public AddAuthorCommandHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public Task<string> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            return _authorService.AddAuthorAsync(request)!;
        }
    }
}

public sealed class AddAuthorRequestValidator : AbstractValidator<AddAuthor.AddAuthorCommand>
{
    public AddAuthorRequestValidator()
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

public sealed class AddAuthorModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/authors", async (AddAuthor.AddAuthorRequest addAuthorRequest, ISender sender) =>
        {
            var cmd = addAuthorRequest.Adapt<AddAuthor.AddAuthorCommand>();

            var result = await sender.Send(cmd);

            return Results.Created($"/authors/{result}", result);
        });
    }
}