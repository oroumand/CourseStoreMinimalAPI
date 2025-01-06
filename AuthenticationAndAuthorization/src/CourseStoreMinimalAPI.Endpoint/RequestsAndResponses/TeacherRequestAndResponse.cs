using AutoMapper;
using CourseStoreMinimalAPI.ApplicationServices;
using CourseStoreMinimalAPI.Endpoint.Infrastructures;
using Microsoft.AspNetCore.OutputCaching;
using System.Reflection.Metadata.Ecma335;

namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;
public class TeacherSaveParameters
{
    public TeacherService teacherService { get; set; }
    public IMapper Mapper { get; set; }
    public IOutputCacheStore CacheStore { get; set; }
    public IFileAdapter FileAdapter { get; set; }
}
public class TeacherSaveRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDay { get; set; }
    public IFormFile? File { get; set; }
}


public class TeacherResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDay { get; set; }
    public string ImageUrl { get; set; }
}