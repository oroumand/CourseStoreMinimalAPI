namespace CourseStoreMinimalAPI.Endpoint.Infrastructures;

public interface IFileAdapter
{
    public string InsertFile(IFormFile file, string path);
    string DeleteFile(string fileName, string path);
    string Update(string oldFileName, IFormFile file, string path)
    {
        DeleteFile(oldFileName, path);
        return InsertFile(file, path);
    }
}
