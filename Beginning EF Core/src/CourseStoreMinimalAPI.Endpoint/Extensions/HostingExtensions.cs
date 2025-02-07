﻿using CourseStoreMinimalAPI.ApplicationServices;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Endpoint.Endpoints;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using AutoMapper;
namespace CourseStoreMinimalAPI.Endpoint.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureService(this WebApplicationBuilder builder)
    {
        AddRequestLog(builder);
        builder.Services.AddScoped<CategoryService>();
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
        app.UseOutputCache();

        app.MapGet("/", () => "Hello World!");

        app.MapCategories("/cagegories");

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
