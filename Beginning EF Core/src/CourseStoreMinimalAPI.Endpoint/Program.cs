using CourseStoreMinimalAPI.ApplicationServices;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
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
string connectionString = builder.Configuration.GetConnectionString("CourseCnn");
builder.Services.AddDbContext<CourseDbContext>(c =>
{
    c.UseSqlServer(connectionString);
});
var app = builder.Build();
//PipeLine
app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpLogging();
app.UseOutputCache();
app.MapGet("/", () => "Hello World!");
app.MapGet("/categoris", async (CategoryService categoryService) =>
{
    var result = await categoryService.GetCategoryAsync();
    return Results.Ok<List<Category>>(result);
}).CacheOutput(c =>
{
    c.Expire(TimeSpan.FromMinutes(15)).Tag("categoris");
});


app.MapGet("/categoris/{id:int}", async (CategoryService categoryService, int id) =>
{
    var result = await categoryService.GetCategoryAsync(id);
    if (result == null)
    {
        return Results.NotFound();
    }
    return Results.Ok<Category>(result);
});
app.MapPost("/categories", async (CategoryService categoryService,IOutputCacheStore cacheStore, Category category) =>
{
    var result = await categoryService.InsertAsync(category);
    await cacheStore.EvictByTagAsync("categoris",default);
    return Results.Created($"/categories/{result}", category);
});

app.Run();
