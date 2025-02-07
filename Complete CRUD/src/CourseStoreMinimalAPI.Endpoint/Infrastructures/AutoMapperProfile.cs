﻿using AutoMapper;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;
using CourseStoreMinimalAPI.Entities;

namespace CourseStoreMinimalAPI.Endpoint.Infrastructures;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CategoryRequest, Category>().ReverseMap();
        CreateMap<CategoryResponse, Category>().ReverseMap();
        CreateMap<TeacherSaveRequest, Teacher>().ReverseMap();
        CreateMap<TeacherResponse, Teacher>().ReverseMap();
        CreateMap<CourseResponse, Course>().ReverseMap();
        CreateMap<CourseSaveRequest, Course>().ReverseMap();
        CreateMap<CourseWithCommentResponse, Course>().ReverseMap();
        CreateMap<CommentResponse, Comment>().ReverseMap();
        CreateMap<CommentRequest, Comment>().ReverseMap();

    }
}
