using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseStoreMinimalAPI.Dal
{
    public class CourseDbContext : DbContext
    {
        public CourseDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseTeacher> CourseTeachers { get; set; }
    }
}
