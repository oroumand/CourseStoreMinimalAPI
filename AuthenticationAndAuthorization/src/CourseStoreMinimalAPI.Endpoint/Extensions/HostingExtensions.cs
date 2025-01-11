using CourseStoreMinimalAPI.ApplicationServices;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Endpoint.Endpoints;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using AutoMapper;
using CourseStoreMinimalAPI.Endpoint.Infrastructures;
using FluentValidation;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;
using CourseStoreMinimalAPI.DAL.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
namespace CourseStoreMinimalAPI.Endpoint.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureService(this WebApplicationBuilder builder)
    {
        AddRequestLog(builder);
        builder.Services.AddScoped<CategoryService>();
        builder.Services.AddScoped<TeacherService>();
        builder.Services.AddScoped<CourseService>();
        builder.Services.AddScoped<CommentService>();
        builder.Services.AddScoped<IFileAdapter, LocalFileStorageAdapter>();

        //builder.Services.AddOutputCache();
        builder.Services.AddAuthentication("Bearer").AddJwtBearer(c =>
        {
            c.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });
        builder.Services.AddAuthorization();
        var redisCnn = builder.Configuration.GetConnectionString("redis");
        builder.Services.AddStackExchangeRedisOutputCache(c =>
        {
            c.Configuration = redisCnn;
        });

        builder.Services.AddOpenApi();
        builder.Services.AddAutoMapper(typeof(HostingExtensions));
        builder.Services.AddValidatorsFromAssembly(typeof(CategoryRequestValidator).Assembly);
        builder.Services.AddHttpContextAccessor();
        AddEfCore(builder);
        builder.Services.AddIdentity<Microsoft.AspNetCore.Identity.IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<CourseDbContext>().AddDefaultTokenProviders();
        builder.Services.AddScoped<UserManager<Microsoft.AspNetCore.Identity.IdentityUser>>();
        builder.Services.AddScoped<SignInManager<Microsoft.AspNetCore.Identity.IdentityUser>>();
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
        app.UseHttpLogging();
        app.UseStaticFiles();
        app.UseOutputCache();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => "Hello World!");

        app.MapCategories("/categories");
        app.MapUsers("/users");
        app.MapTeachers("/teachers");
        app.MapCourses("/courses");
        app.MapComments("/comments");


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
