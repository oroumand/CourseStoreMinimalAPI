using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseStoreMinimalAPI.Dal.Configurations
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(c => c.Title).HasMaxLength(150);
            builder.Property(c => c.ImageUrl).HasMaxLength(1000);

        }
    }
}
