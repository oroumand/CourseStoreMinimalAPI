using AutoMapper;
using CourseStoreMinimalAPI.ApplicationServices;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;
using CourseStoreMinimalAPI.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseStoreMinimalAPI.Endpoint.Endpoints;

public static class CategoryEndpoints
{
    static string CacheKey = "categoris";
    public static WebApplication MapCategories(this WebApplication app, string prefix)
    {
        var categoryGroup = app.MapGroup(prefix);
        categoryGroup.MapGet("/", GetList).CacheOutput(c => { c.Expire(TimeSpan.FromMinutes(15)).Tag(CacheKey); });
        categoryGroup.MapGet("/{id:int}", GetById);
        categoryGroup.MapPost("/", Insert);
        categoryGroup.MapPut("/{id:int}", Update);
        categoryGroup.MapDelete("/{id:int}", Delete);
        return app;
    }
    static async Task<Ok<List<CategoryResponse>>> GetList(CategoryService categoryService,IMapper mapper)
    {
        var result = await categoryService.GetCategoryAsync();
        var response = mapper.Map<List<CategoryResponse>>(result);
        return TypedResults.Ok<List<CategoryResponse>>(response);
    }


    static async Task<Results<NotFound, Ok<CategoryResponse>>> GetById(CategoryService categoryService, IMapper mapper, int id)
    {
        var result = await categoryService.GetCategoryAsync(id);
        if (result == null)
        {
            return TypedResults.NotFound();
        }
        var response = mapper.Map<CategoryResponse>(result);
        return TypedResults.Ok<CategoryResponse>(response);
    }

    static async Task<Created<CategoryResponse>> Insert(CategoryService categoryService,IMapper mapper, IOutputCacheStore cacheStore, CategoryRequest category)
    {
        var categoryForSave = mapper.Map<Category>(category);
        var result = await categoryService.InsertAsync(categoryForSave);
        await cacheStore.EvictByTagAsync(CacheKey, default);
        var response = mapper.Map<CategoryResponse>(categoryForSave); 
       
        return TypedResults.Created($"/categories/{result}", response);
    }

    static async Task<Results<NoContent, NotFound>> Update(CategoryService categoryService, IMapper mapper, IOutputCacheStore cacheStore, int id, CategoryRequest category)
    {
        if (!await categoryService.Exists(id))
        {
            return TypedResults.NotFound();
        }
        var categoryForSave = mapper.Map<Category>(category);
        await categoryService.Update(categoryForSave);
        await cacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }

    static async Task<Results<NoContent, NotFound>> Delete(CategoryService categoryService, IOutputCacheStore cacheStore, int id)
    {
        if (!await categoryService.Exists(id))
        {
            return TypedResults.NotFound();
        }
        await categoryService.Delete(id);
        await cacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }
}
