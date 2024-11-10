namespace CourseStoreMinimalAPI.WebAPI.DTOs
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }

    }

    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}