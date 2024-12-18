using System.Reflection.Metadata.Ecma335;

namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;

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