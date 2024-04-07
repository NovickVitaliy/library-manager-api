using Carter;
using library_manager_api.DataAccess;
using library_manager_api.DataAccess.Abstraction;
using library_manager_api.Options;
using library_manager_api.OtherConfiguration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongoDbOptions = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    
    ArgumentNullException.ThrowIfNull(mongoDbOptions, nameof(mongoDbOptions));
    
    return new MongoClient(mongoDbOptions.ConnectionString);
});

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

builder.Services.AddCarter();
MapsterConfiguration.ConfigureMaps();
var app = builder.Build();

app.MapCarter();

app.Run();