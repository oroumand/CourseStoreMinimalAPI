using CourseStoreMinimalAPI.Dal;
using CourseStoreMinimalAPI.Services;
using CourseStoreMinimalAPI.WebAPI.Endpoints;
using CourseStoreMinimalAPI.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();
var app = builder.Build();
app.ConfigurePipeline();
app.Run();
