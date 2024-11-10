using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.Entities
{
    public sealed class Course : BaseEntity<int>
    {
        public string Title { get; set; } = null!;
        public bool IsInProgress { get; set; }
        public DateTime StartDate { get; set; }
        public string? ImageUrl { get; set; }
        public List<Comment> Comments { get; set; } = [];
        public List<CourseCategory> CourseCategories { get; set; } = [];
        public List<CourseTeacher> CourseTeachers { get; set; } = [];
    }
}
