using System.Text.Json;
using Carter;
using FluentValidation;
using library_manager_api.DataAccess;
using library_manager_api.DataAccess.Abstraction;
using library_manager_api.Exceptions;
using library_manager_api.MediatrBehaviours;
using library_manager_api.Options;
using library_manager_api.OtherConfiguration;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongoDbOptions = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    
    ArgumentNullException.ThrowIfNull(mongoDbOptions, nameof(mongoDbOptions));
    
    return new MongoClient(mongoDbOptions.ConnectionString);
});

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddSingleton<IAuthorService, AuthorService>();

builder.Services.AddOptions<MongoDbSettings>()
    .Bind(builder.Configuration.GetSection(MongoDbSettings.Position))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

builder.Services.AddCarter();

MapsterConfiguration.ConfigureMaps();

var app = builder.Build();

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Use(async (HttpContext context, RequestDelegate next) =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error is BaseException exception)
        {
            var apiResponse = exception.ToApiResponse();
            var json = apiResponse.ToJson();

            context.Response.StatusCode = apiResponse.StatusCode;
            await context.Response.WriteAsync(json);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Internal Server Error");
        }
    });
});

app.MapCarter();

app.Run();