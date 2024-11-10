namespace CourseStoreMinimalAPI.Entities
{
    public sealed class CourseCategory : BaseEntity<int>
    {
        public int CourseId { get; set; }
        public int CategoryId { get; set; }
        public Course Course { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
