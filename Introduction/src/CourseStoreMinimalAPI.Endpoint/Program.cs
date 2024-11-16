using CourseStoreMinimalAPI.ApplicationServices;
using CourseStoreMinimalAPI.Entities;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);
//Services
builder.Services.AddHttpLogging(c =>
{
    c.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});
builder.Logging.AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information);
builder.Services.AddScoped<CategoryService>();
builder.Services.AddOutputCache();
builder.Services.AddOpenApi();

var app = builder.Build();
//PipeLine
app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpLogging();
app.UseOutputCache();
app.MapGet("/", () => "Hello World!");
app.MapGet("/categoris", (CategoryService categoryService) =>
{
    return Results.Ok<List<Category>>(categoryService.GetCategories());
}).CacheOutput(c =>
{
    c.Expire(TimeSpan.FromMinutes(15));
});

app.Run();
