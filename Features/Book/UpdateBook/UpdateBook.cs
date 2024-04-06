using Carter;
using FluentValidation;
using library_manager_api.ApiResponse;
using library_manager_api.CQRS.Command;
using library_manager_api.DataAccess.Abstraction;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace library_manager_api.Features.Book.UpdateBook;

public static class UpdateBook
{
    public sealed record UpdateBookRequest(
        string Title,
        string Description,
        string Language,
        ushort YearPublished,
        IEnumerable<string> Categories,
        ushort Pages);

    public sealed record UpdateBookCommand(
        string Id,
        string Title,
        string Description,
        string Language,
        ushort YearPublished,
        IEnumerable<string> Categories,
        ushort Pages) : ICommand;

    public sealed class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
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

    public sealed class UpdateBookCommandHandler : ICommandHandler<UpdateBookCommand>
    {
        private readonly IBookService _bookService;

        public UpdateBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = request.Adapt<Models.Book>();

            await _bookService.UpdateBookAsync(request.Id, book);

            return Unit.Value;
        }
    }
}

public sealed class UpdateBookModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/books/{id}",
            async ([FromRoute] string id, [FromBody] UpdateBook.UpdateBookRequest updateBookRequest, ISender sender) =>
            {
                var updateBookRequestValidator = new UpdateBook.UpdateBookRequestValidator();
                var validationResult = await updateBookRequestValidator.ValidateAsync(updateBookRequest);

                if (!validationResult.IsValid)
                {
                    var validationErrors = validationResult.Errors.GroupBy(
                        x => x.PropertyName,
                        x => x.ErrorMessage)
                        .ToDictionary(x => x.Key, x => x.ToList());

                    return Results.Json(new ValidationFailtureApiResponse()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Description = "Validation Error",
                        ValidationErrors = validationErrors
                    });
                }

                var updateBookCommand = new UpdateBook.UpdateBookCommand(
                    id,
                    updateBookRequest.Title,
                    updateBookRequest.Description,
                    updateBookRequest.Language,
                    updateBookRequest.YearPublished,
                    updateBookRequest.Categories,
                    updateBookRequest.Pages);

                await sender.Send(updateBookCommand);

                return Results.NoContent();
            });
    }
}