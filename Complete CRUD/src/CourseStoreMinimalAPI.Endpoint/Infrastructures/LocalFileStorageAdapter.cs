namespace CourseStoreMinimalAPI.Endpoint.Infrastructures;

public class LocalFileStorageAdapter(IHostEnvironment environment) : IFileAdapter
{
    private readonly IHostEnvironment _environment = environment;

    public string DeleteFile(string fileName, string path)
    {
        string webRootPath = _environment.ContentRootPath;
        string finalFilePath = Path.Combine(webRootPath, path);
        if (File.Exists(finalFilePath))
        {
            File.Delete(finalFilePath);
            return finalFilePath;
        }
        return string.Empty;
    }

    public string InsertFile(IFormFile file, string path)
    {
        string webRootPath = _environment.ContentRootPath;
        string fileExtension = Path.GetExtension(file.FileName);
        string fileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
        string folderForSave = Path.Combine(webRootPath, path);
        if (!Directory.Exists(folderForSave))
        {
            Directory.CreateDirectory(folderForSave);
            //throw new DirectoryNotFoundException(path);
        }

        using MemoryStream memoryStream = new();
        file.CopyTo(memoryStream);
        string finalFilePath = Path.Combine(folderForSave, fileName);
        File.WriteAllBytes(finalFilePath, memoryStream.ToArray());

        return fileName;
    }
}

