namespace CourseStoreMinimalAPI.Entities
{
    public sealed class CourseTeacher : BaseEntity<int>
    {
        public int TeacherId { get; set; }
        public int CourseId { get; set; }
        public Teacher Teacher { get; set; }
        public Course Course { get; set; }
        public int Order { get; set; }
    }
}
