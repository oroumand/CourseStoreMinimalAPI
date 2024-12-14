using AutoMapper;
using CourseStoreMinimalAPI.ApplicationServices;
using CourseStoreMinimalAPI.Endpoint.Infrastructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;
using CourseStoreMinimalAPI.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseStoreMinimalAPI.Endpoint.Endpoints;

public static class CommentEndpoints
{
    static string CacheKey = "comments";
    private static string routePrefix;
    public static WebApplication MapComments(this WebApplication app, string prefix)
    {
        var teacherGroup = app.MapGroup(prefix);
        routePrefix = prefix;
        teacherGroup.MapGet("/{courseId:int}", GetList);//.CacheOutput(c => { c.Expire(TimeSpan.FromMinutes(15)).Tag(CacheKey); });
        teacherGroup.MapGet("/totalCount/{courseId:int}", TotalCount);
        teacherGroup.MapGet("/Get/{id:int}", GetById);
        teacherGroup.MapPost("/", Insert).DisableAntiforgery();
        teacherGroup.MapPut("/{id:int}", Update).DisableAntiforgery();
        teacherGroup.MapDelete("/{id:int}", Delete);
        return app;
    }

    static async Task<Ok<List<CommentResponse>>> GetList(CommentService commentService, IMapper mapper, int courseId)
    {
        var result = await commentService.GetAllAsync(courseId);
        var response = mapper.Map<List<CommentResponse>>(result);
        return TypedResults.Ok<List<CommentResponse>>(response);
    }

    static async Task<Ok<int>> TotalCount(CommentService commentService, int courseId)
    {
        int totalCount = await commentService.GetTotalCountAsync(courseId);
        return TypedResults.Ok<int>(totalCount);

    }


    static async Task<Results<NotFound, Ok<CommentResponse>>> GetById(CommentService commentService, IMapper mapper, int id)
    {
        var result = await commentService.GetCommentAsync(id);
        if (result == null)
        {
            return TypedResults.NotFound();
        }
        var response = mapper.Map<CommentResponse>(result);
        return TypedResults.Ok<CommentResponse>(response);
    }
    static async Task<Created<CommentResponse>> Insert(CommentService commentService, IMapper mapper, IOutputCacheStore cacheStore, IFileAdapter fileAdapter, [FromForm] CommentRequest request)
    {
        Comment comment = mapper.Map<Comment>(request);

        int savedEntityId = await commentService.Insert(comment);
        await cacheStore.EvictByTagAsync(CacheKey, default);

        var createdCourseResponse = mapper.Map<CommentResponse>(comment);
        return TypedResults.Created($"{routePrefix}/{savedEntityId}", createdCourseResponse);
    }

    static async Task<Results<NoContent, NotFound>> Update(CommentService commentService, IMapper mapper, IOutputCacheStore cacheStore, IFileAdapter fileAdapter, int id, [FromForm] CommentRequest request)
    {
        if (!await commentService.Exists(id))
        {
            return TypedResults.NotFound();
        }
        var comment = await commentService.GetCommentAsync(id);
        comment.CommentDate = request.CommentDate;
        comment.CommentBody = request.CommentBody;
        comment.CourseId = request.CourseId;
        await commentService.Update();
        await cacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }

    static async Task<Results<NoContent, NotFound>> Delete(CommentService commentService, IFileAdapter fileAdapter, IOutputCacheStore cacheStore, int id)
    {
        if (!await commentService.Exists(id))
        {
            return TypedResults.NotFound();
        }
        var course = await commentService.GetCommentAsync(id);

        await commentService.Delete(course);
        await cacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }
}
