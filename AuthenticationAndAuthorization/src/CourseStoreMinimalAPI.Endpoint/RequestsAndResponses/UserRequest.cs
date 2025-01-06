using CourseStoreMinimalAPI.Entities;
using System.Reflection.Metadata.Ecma335;

namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;

public class UserRegistrationRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
public class UserRegistrationResponse
{
    public Boolean IsOk { get; set; }
}