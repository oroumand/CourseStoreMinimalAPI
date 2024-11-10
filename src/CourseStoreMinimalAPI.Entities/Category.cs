namespace CourseStoreMinimalAPI.Entities
{
    public class Category : BaseEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CourseCategory> CourseCategories { get; set; } = [];

    }
}
