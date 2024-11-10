using CourseStoreMinimalAPI.Dal;
using CourseStoreMinimalAPI.Services;
using CourseStoreMinimalAPI.WebAPI.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CourseStoreMinimalAPI.WebAPI.Extensions
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            IConfiguration configuration = builder.Configuration;
            builder.Services.AddOutputCache();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<CourseDbContext>
                (c => c.UseSqlServer(configuration.GetConnectionString("StoreDb")));

            builder.Services.AddScoped<CategoryServices>();
            builder.Services.AddAutoMapper(typeof(Program));

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {

            app.MapCategories("/categories");

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapGet("/", () => "Hello World");
            return app;
        }
    }
}
