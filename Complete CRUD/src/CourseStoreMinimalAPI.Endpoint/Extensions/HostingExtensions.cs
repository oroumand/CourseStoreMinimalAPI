using CourseStoreMinimalAPI.ApplicationServices;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Endpoint.Endpoints;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using AutoMapper;
using CourseStoreMinimalAPI.Endpoint.Infrastructures;
namespace CourseStoreMinimalAPI.Endpoint.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureService(this WebApplicationBuilder builder)
    {
        AddRequestLog(builder);
        builder.Services.AddScoped<CategoryService>();
        builder.Services.AddScoped<TeacherService>();
        builder.Services.AddScoped<IFileAdapter, LocalFileStorageAdapter>();
        builder.Services.AddOutputCache();
        builder.Services.AddOpenApi();
        builder.Services.AddAutoMapper(typeof(HostingExtensions));
        AddEfCore(builder);
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
        app.UseHttpLogging();
        app.UseStaticFiles();
        app.UseOutputCache();

        app.MapGet("/", () => "Hello World!");

        app.MapCategories("/cagegories");
        app.MapTeachers("/teachers");


        return app;
    }

    private static void AddRequestLog(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpLogging(c =>
        {
            c.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
        });
        builder.Logging.AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information);
    }

    private static void AddEfCore(WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("CourseCnn");
        builder.Services.AddDbContext<CourseDbContext>(c =>
        {
            c.UseSqlServer(connectionString);
        });
    }
}
