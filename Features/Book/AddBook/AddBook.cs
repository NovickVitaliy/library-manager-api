using System.Text.Json;
using Carter;
using FluentValidation;
using library_manager_api.ApiResponse;
using library_manager_api.CQRS.Command;
using library_manager_api.DataAccess.Abstraction;
using library_manager_api.Exceptions;
using library_manager_api.Exceptions.Author;
using library_manager_api.Extensions;
using Mapster;
using MediatR;
using ValidationException = library_manager_api.Exceptions.ValidationException;

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
        private readonly IAuthorService _authorService;

        public AddBookCommandHandler(IBookService bookService, IAuthorService authorService)
        {
            _bookService = bookService;
            _authorService = authorService;
        }

        public async Task<string> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorService.GetAuthorByIdAsync(request.AuthorId);
            
            if (author is null) throw new AuthorNotFoundException(request.AuthorId);
            
            return (await _bookService.AddBookAsync(request))!;
        }
    }
}

public class AddBookValidator : AbstractValidator<AddBook.AddBookCommand>
{
    public AddBookValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("{PropertyName} must be supplied.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("{PropertyName} must be supplied.");

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

public sealed class AddBookModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/books", async (AddBook.AddBookRequest addBookRequest, ISender sender) =>
        {
            var cmd = addBookRequest.Adapt<AddBook.AddBookCommand>();

            var bookId = await sender.Send(cmd);

            return Results.Created($"/products/{bookId}", bookId);
        });
    }
}