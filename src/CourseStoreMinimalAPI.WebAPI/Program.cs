using CourseStoreMinimalAPI.Dal;
using CourseStoreMinimalAPI.Services;
using CourseStoreMinimalAPI.WebAPI.Endpoints;
using CourseStoreMinimalAPI.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.ConfigureServices();
app.ConfigurePipeline();
app.Run();
