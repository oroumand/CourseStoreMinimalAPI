using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseStoreMinimalAPI.Dal.Configurations
{
    internal class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(c => c.Name).HasMaxLength(150);
            builder.Property(c => c.Picture).HasMaxLength(1000);
        }
    }
}
