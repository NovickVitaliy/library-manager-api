using Carter;
using library_manager_api.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<MongoDbSettings>()
    .Bind(builder.Configuration.GetSection(MongoDbSettings.Position))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddCarter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCarter();

app.Run();