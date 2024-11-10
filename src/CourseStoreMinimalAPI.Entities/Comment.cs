namespace CourseStoreMinimalAPI.Entities
{
    public sealed class Comment : BaseEntity<int>
    {
        public string Body { get; set; } = null!;
        public int CourseId { get; set; }
    }
}
