using System.Text.Json;
using Carter;
using FluentValidation;
using library_manager_api.ApiResponse;
using library_manager_api.CQRS.Command;
using library_manager_api.DataAccess.Abstraction;
using library_manager_api.Extensions;
using Mapster;
using MediatR;

namespace library_manager_api.Features.Book.AddBook;

public static class AddBook
{
    public sealed record AddBookRequest(
        string Title,
        string Description,
        string Language,
        short YearPublished,
        ICollection<string> Categories,
        short Pages, 
        string AuthorId);

    public class AddBookValidator : AbstractValidator<AddBookRequest>
    {
        public AddBookValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("{PropertyName} must be supplied.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("{PropertyName} nust be supplied.");

            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("{PropertyName} must be supplied");

            RuleFor(x => x.YearPublished)
                .GreaterThan((short)0).WithMessage("{PropertyName} cannot be less than or equal to 0")
                .WithName("Year Published");

            RuleFor(x => x.Categories)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty");

            RuleFor(x => x.Pages)
                .GreaterThan((short)0).WithMessage("{PropertyName} cannot be less than or equal to 0");
        }
    }

    public sealed record AddBookCommand(
        string Title,
        string Description,
        string Language,
        short YearPublished,
        ICollection<string> Categories,
        short Pages,
        string AuthorId) : ICommand<string>;

    public sealed class AddBookCommandHandler : ICommandHandler<AddBookCommand, string>
    {
        private readonly IBookService _bookService;

        public AddBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<string> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            return await _bookService.AddBookAsync(request);
        }
    }
}

public sealed class AddBookModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/books", async (AddBook.AddBookRequest addBookRequest, ISender sender) =>
        {
            var validator = new AddBook.AddBookValidator();
            var validationResult = await validator.ValidateAsync(addBookRequest);

            if (!validationResult.IsValid)
            {
                var response = validationResult.Errors.ToValidationFailureApiResponse();
                
                return Results.Json(response, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            
            var cmd = addBookRequest.Adapt<AddBook.AddBookCommand>();

            var bookId = await sender.Send(cmd);

            return Results.Created($"/products/{bookId}", bookId);
        });
    }
}