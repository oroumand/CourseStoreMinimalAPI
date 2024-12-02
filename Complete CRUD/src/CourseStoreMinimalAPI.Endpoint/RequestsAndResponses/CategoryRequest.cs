using CourseStoreMinimalAPI.Entities;
using System.Reflection.Metadata.Ecma335;

namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;

public class CategoryRequest
{
    public string Name { get; set; }
}
public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
}