using AutoMapper;
using CourseStoreMinimalAPI.Entities;
using CourseStoreMinimalAPI.Services;
using CourseStoreMinimalAPI.WebAPI.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CourseStoreMinimalAPI.WebAPI.Endpoints
{
    public static class CategoryEndpoints
    {
        public static WebApplication MapCategories(this WebApplication app, string groupRout)
        {
            var group = app.MapGroup(groupRout);
            group.MapGet("/", GetCategory);
            group.MapGet("/{Id:int}", GetById);
            group.MapPost("/", Create);
            group.MapPut("/{Id:int}", Update);
            group.MapDelete("/{Id:int}", Delete);

            return app;
        }

        static async Task<Ok<List<Category>>> GetCategory(CategoryServices services)
        {
            var categories = await services.GetAll();
            return TypedResults.Ok(categories);

        }

        static async Task<Results<Ok<Category>, NotFound>> GetById(CategoryServices services, int Id)
        {
            var category = await services.GetById(Id);
            if (category == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(category);
        }

        static async Task<Created<Category>> Create(CategoryServices services, IMapper mapper, CreateCategoryRequest request)
        {
            var category = mapper.Map<Category>(request);
            var result = await services.Create(category);
            return TypedResults.Created($"/categories/{result}", category);
        }

        static async Task<Results<NotFound, NoContent>> Update(CategoryServices services, int id, Category category)
        {
            var exists = await services.Exists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }
            category.Id = id;
            await services.Update(category);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> Delete(CategoryServices services, int id)
        {
            var exists = await services.Exists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            await services.Delete(id);
            return TypedResults.NoContent();
        }

    }
}
