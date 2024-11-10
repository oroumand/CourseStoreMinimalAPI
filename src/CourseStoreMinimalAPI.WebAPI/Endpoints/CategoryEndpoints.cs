using AutoMapper;
using CourseStoreMinimalAPI.Entities;
using CourseStoreMinimalAPI.Services;
using CourseStoreMinimalAPI.WebAPI.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseStoreMinimalAPI.WebAPI.Endpoints
{
    public static class CategoryEndpoints
    {
        const string CacheKey = "CATEGORIES_GET";
        public static WebApplication MapCategories(this WebApplication app, string groupRout)
        {
            var group = app.MapGroup(groupRout);
            group.MapGet("/", GetCategory).CacheOutput().WithTags(CacheKey);
            group.MapGet("/{Id:int}", GetById);
            group.MapPost("/", Create);
            group.MapPut("/{Id:int}", Update);
            group.MapDelete("/{Id:int}", Delete);

            return app;
        }

        static async Task<Ok<List<CategoryResponse>>> GetCategory(CategoryServices services,                                                                  
                                                                  IMapper mapper)
        {
            var categories = await services.GetAll();
            var response = mapper.Map<List<CategoryResponse>>(categories);
            return TypedResults.Ok(response);

        }

        static async Task<Results<Ok<CategoryResponse>, NotFound>> GetById(CategoryServices services,
                                                                           IMapper mapper,
                                                                           int Id)
        {
            var category = await services.GetById(Id);
            if (category == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(mapper.Map<CategoryResponse>(category));
        }

        static async Task<Created<CategoryResponse>> Create(CategoryServices services,
                                                            IOutputCacheStore outputCacheStore,
                                                            IMapper mapper,
                                                            CreateCategoryRequest request)
        {
            var category = mapper.Map<Category>(request);
            var result = await services.Create(category);
            await outputCacheStore.EvictByTagAsync(CacheKey,default);
            return TypedResults.Created($"/categories/{result}", mapper.Map<CategoryResponse>(category));
        }

        static async Task<Results<NotFound, NoContent>> Update(CategoryServices services,
                                                               IOutputCacheStore outputCacheStore,
                                                               IMapper mapper,
                                                               int id,
                                                               CreateCategoryRequest request)
        {
            var exists = await services.Exists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }
            var category = mapper.Map<Category>(request);
            category.Id = id;
            await services.Update(category);
            await outputCacheStore.EvictByTagAsync(CacheKey, default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> Delete(CategoryServices services,
                                                               IOutputCacheStore outputCacheStore,
                                                               int id)
        {
            var exists = await services.Exists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            await services.Delete(id);
            await outputCacheStore.EvictByTagAsync(CacheKey, default);
            return TypedResults.NoContent();
        }

    }
}
