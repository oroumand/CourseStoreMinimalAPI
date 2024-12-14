using AutoMapper;
using CourseStoreMinimalAPI.ApplicationServices;
using CourseStoreMinimalAPI.Endpoint.Infrastructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;
using CourseStoreMinimalAPI.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseStoreMinimalAPI.Endpoint.Endpoints;

public static class CourseEndpoints
{
    static string CacheKey = "courses";
    static string CourseImageFolder = @"Images\Course";
    static string DefaultCourseImageName = "Default.jpg";
    private static string routePrefix;
    public static WebApplication MapCourses(this WebApplication app, string prefix)
    {
        var teacherGroup = app.MapGroup(prefix);
        routePrefix = prefix;
        teacherGroup.MapGet("/{pageNumber:int}/{itemPerPage:int}", GetList);//.CacheOutput(c => { c.Expire(TimeSpan.FromMinutes(15)).Tag(CacheKey); });
        teacherGroup.MapGet("/totalCount", TotalCount);
        teacherGroup.MapGet("/search", Search);
        teacherGroup.MapGet("/{id:int}", GetById);
        teacherGroup.MapPost("/", Insert).DisableAntiforgery();
        teacherGroup.MapPut("/{id:int}", Update).DisableAntiforgery();
        teacherGroup.MapDelete("/{id:int}", Delete);
        return app;
    }

    static async Task<Ok<List<CourseResponse>>> GetList(CourseService courseService, IMapper mapper, int pageNumber, int itemPerPage)
    {
        var result = await courseService.GetAllAsync(pageNumber, itemPerPage);
        var response = mapper.Map<List<CourseResponse>>(result);
        return TypedResults.Ok<List<CourseResponse>>(response);
    }

    static async Task<Ok<int>> TotalCount(CourseService courseService)
    {
        int totalCount = await courseService.GetTotalCountAsync();
        return TypedResults.Ok<int>(totalCount);

    }

    static async Task<Ok<List<CourseResponse>>> Search(CourseService courseService, IMapper mapper, string? title, bool? isOnline)
    {
        var result = await courseService.SearchAsync(title,isOnline);
        var response = mapper.Map<List<CourseResponse>>(result);
        return TypedResults.Ok<List<CourseResponse>>(response);
    }

    static async Task<Results<NotFound, Ok<CourseResponse>>> GetById(CourseService courseService, IMapper mapper, int id)
    {
        var result = await courseService.GetCourseAsync(id);
        if (result == null)
        {
            return TypedResults.NotFound();
        }
        var response = mapper.Map<CourseResponse>(result);
        return TypedResults.Ok<CourseResponse>(response);
    }
    static async Task<Created<CourseResponse>> Insert(CourseService courseService, IMapper mapper, IOutputCacheStore cacheStore, IFileAdapter fileAdapter, [FromForm] CourseSaveRequest request)
    {
        Course course = mapper.Map<Course>(request);
        string fileName = DefaultCourseImageName;
        if (request.File is not null)
        {
            fileName = fileAdapter.InsertFile(request.File, CourseImageFolder);
        }
        course.ImageUrl = fileName;
        int savedEntityId = await courseService.Insert(course);
        await cacheStore.EvictByTagAsync(CacheKey, default);

        var createdCourseResponse = mapper.Map<CourseResponse>(course);
        return TypedResults.Created($"{routePrefix}/{savedEntityId}", createdCourseResponse);


    }

    static async Task<Results<NoContent, NotFound>> Update(CourseService courseService, IMapper mapper, IOutputCacheStore cacheStore, IFileAdapter fileAdapter, int id, [FromForm] CourseSaveRequest request)
    {
        if (!await courseService.Exists(id))
        {
            return TypedResults.NotFound();
        }
        var course = await courseService.GetCourseAsync(id);
        course.Title=request.Title;
        course.Description=request.Description;
        course.EndDate=request.EndDate;
        course.StartDate=request.StartDate;
        course.IsOnline=request.IsOnline;

        if (request.File is not null)
        {
            course.ImageUrl = fileAdapter.Update(course.ImageUrl, request.File, CourseImageFolder);
        }

        await courseService.Update();
        await cacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }

    static async Task<Results<NoContent, NotFound>> Delete(CourseService courseService, IFileAdapter fileAdapter, IOutputCacheStore cacheStore, int id)
    {
        if (!await courseService.Exists(id))
        {
            return TypedResults.NotFound();
        }
        var course = await courseService.GetCourseAsync(id);
        fileAdapter.DeleteFile(course.ImageUrl, CourseImageFolder);

        await courseService.Delete(course);
        await cacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }
}
