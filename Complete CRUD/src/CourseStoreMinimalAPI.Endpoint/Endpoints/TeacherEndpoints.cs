using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using CourseStoreMinimalAPI.ApplicationServices;
using AutoMapper;
using Microsoft.AspNetCore.OutputCaching;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;
using Microsoft.AspNetCore.Mvc;
using CourseStoreMinimalAPI.Entities;
using CourseStoreMinimalAPI.Endpoint.Infrastructures;
using Azure;

namespace CourseStoreMinimalAPI.Endpoint.Endpoints;

public static class TeacherEndpoints
{
    static string CacheKey = "teachers";
    static string TeacherImageFolder = @"Images\Teachers";
    static string DefaultTeacherImageName = "Default.jpg";
    private static string routePrefix;
    public static WebApplication MapTeachers(this WebApplication app, string prefix)
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

    static async Task<Ok<List<TeacherResponse>>> GetList(TeacherService teacherService, IMapper mapper, int pageNumber, int itemPerPage)
    {
        var result = await teacherService.GetAllAsync(pageNumber, itemPerPage);
        var response = mapper.Map<List<TeacherResponse>>(result);
        return TypedResults.Ok<List<TeacherResponse>>(response);
    }

    static async Task<Ok<int>> TotalCount(TeacherService teacherService)
    {
        int totalCount = await teacherService.GetTotalCountAsync();
        return TypedResults.Ok<int>(totalCount);

    }

    static async Task<Ok<List<TeacherResponse>>> Search(TeacherService teacherService, IMapper mapper, string? firstName, string? lastName)
    {
        var result = await teacherService.SearchAsync(firstName, lastName);
        var response = mapper.Map<List<TeacherResponse>>(result);
        return TypedResults.Ok<List<TeacherResponse>>(response);
    }

    static async Task<Results<NotFound, Ok<TeacherResponse>>> GetById(TeacherService teacherService, IMapper mapper, int id)
    {
        var result = await teacherService.GetTeacherAsync(id);
        if (result == null)
        {
            return TypedResults.NotFound();
        }
        var response = mapper.Map<TeacherResponse>(result);
        return TypedResults.Ok<TeacherResponse>(response);
    }
    static async Task<Created<TeacherResponse>> Insert(TeacherService teacherService, IMapper mapper, IOutputCacheStore cacheStore, IFileAdapter fileAdapter, [FromForm] TeacherSaveRequest request)
    {
        Teacher teacher = mapper.Map<Teacher>(request);
        string fileName = DefaultTeacherImageName;
        if (request.File is not null)
        {
            fileName = fileAdapter.InsertFile(request.File, TeacherImageFolder);
        }
        teacher.ImageUrl = fileName;
        int savedEntityId = await teacherService.Insert(teacher);
        await cacheStore.EvictByTagAsync(CacheKey, default);

        var createdTeacherResponse = mapper.Map<TeacherResponse>(teacher);
        return TypedResults.Created($"{routePrefix}/{savedEntityId}", createdTeacherResponse);


    }

    static async Task<Results<NoContent, NotFound>> Update(TeacherService teacherService, IMapper mapper, IOutputCacheStore cacheStore, IFileAdapter fileAdapter, int id, [FromForm] TeacherSaveRequest request)
    {
        if (!await teacherService.Exists(id))
        {
            return TypedResults.NotFound();
        }
        var teacher = await teacherService.GetTeacherAsync(id);
        teacher.FirstName = request.FirstName;
        teacher.LastName = request.LastName;
        teacher.Birtday = request.BirthDay;

        if (request.File is not null)
        {
            teacher.ImageUrl = fileAdapter.Update(teacher.ImageUrl, request.File, TeacherImageFolder);
        }

        await teacherService.Update();
        await cacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }

    static async Task<Results<NoContent, NotFound>> Delete(TeacherService teacherService, IFileAdapter fileAdapter, IOutputCacheStore cacheStore, int id)
    {
        if (!await teacherService.Exists(id))
        {
            return TypedResults.NotFound();
        }
        var teacher = await teacherService.GetTeacherAsync(id);
        fileAdapter.DeleteFile(teacher.ImageUrl, TeacherImageFolder);

        await teacherService.Delete(teacher);
        await cacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }
}
