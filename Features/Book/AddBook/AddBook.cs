using System.Text.Json;
using Carter;
using FluentValidation;
using library_manager_api.ApiResponse;
using library_manager_api.CQRS.Command;
using library_manager_api.DataAccess.Abstraction;
using library_manager_api.Models;
using Mapster;
using MediatR;

namespace library_manager_api.Features.AddBook;

public static class AddBook
{
    public sealed record AddBookRequest(
        string Title,
        string Description,
        string Language,
        ushort YearPublished,
        ICollection<string> Categories,
        ushort Pages);

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
                .GreaterThan((ushort)0).WithMessage("{PropertyName} cannot be less than or equal to 0")
                .WithName("Year Published");

            RuleFor(x => x.Categories)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty");

            RuleFor(x => x.Pages)
                .GreaterThan((ushort)0).WithMessage("{PropertyName} cannot be less than or equal to 0");
        }
    }

    public sealed record AddBookCommand(
        string Title,
        string Description,
        string Language,
        ushort YearPublished,
        ICollection<string> Categories,
        ushort Pages) : ICommand<string>;

    public sealed class AddBookCommandHandler : ICommandHandler<AddBookCommand, string>
    {
        private readonly IBookService _bookService;

        public AddBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<string> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var book = request.Adapt<Book>();
            
            return await _bookService.AddBookAsync(book);
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
                var validationErrors = validationResult.Errors.GroupBy(
                    x => x.PropertyName,
                    x => x.ErrorMessage)
                    .ToDictionary(
                        x => x.Key, 
                        x => x.ToList());
                var response = new ValidationFailtureApiResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Description = "Validation Error",
                    ValidationErrors = validationErrors 
                };

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