using AutoMapper;
using CourseStoreMinimalAPI.Entities;
using CourseStoreMinimalAPI.WebAPI.DTOs;

namespace CourseStoreMinimalAPI.WebAPI.Infrastructures
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Category, CreateCategoryRequest>().ReverseMap();
            CreateMap<Category, CategoryResponse>().ReverseMap();
        }
    }
}
