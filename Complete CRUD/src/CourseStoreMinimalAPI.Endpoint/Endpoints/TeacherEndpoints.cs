﻿using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using CourseStoreMinimalAPI.ApplicationServices;
using AutoMapper;
using Microsoft.AspNetCore.OutputCaching;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;
using Microsoft.AspNetCore.Mvc;
using CourseStoreMinimalAPI.Entities;
using CourseStoreMinimalAPI.Endpoint.Infrastructures;

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
        //teacherGroup.MapGet("/", GetList).CacheOutput(c => { c.Expire(TimeSpan.FromMinutes(15)).Tag(CacheKey); });
        //teacherGroup.MapGet("/{id:int}", GetById);
        teacherGroup.MapPost("/", Insert).DisableAntiforgery();
        //teacherGroup.MapPut("/{id:int}", Update);
        //teacherGroup.MapDelete("/{id:int}", Delete);
        return app;
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
}