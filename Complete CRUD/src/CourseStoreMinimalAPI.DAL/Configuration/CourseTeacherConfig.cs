using CourseStoreMinimalAPI.ApplicationServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseStoreMinimalAPI.DAL.Configuration;

public class CourseTeacherConfig : IEntityTypeConfiguration<CourseTeacher>
{
    public void Configure(EntityTypeBuilder<CourseTeacher> builder)
    {
        builder.HasKey(c => new { c.CourseId, c.TeacherId });
    }
}