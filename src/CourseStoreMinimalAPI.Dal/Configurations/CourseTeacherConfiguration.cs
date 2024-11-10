using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseStoreMinimalAPI.Dal.Configurations
{
    internal class CourseTeacherConfiguration : IEntityTypeConfiguration<CourseTeacher>
    {
        public void Configure(EntityTypeBuilder<CourseTeacher> builder)
        {
        }
    }
}
