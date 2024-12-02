using AutoMapper;
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
    }
}
