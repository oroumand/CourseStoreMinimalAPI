using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.DAL.Configuration;
public class CourseConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(c => c.Title).HasMaxLength(200);
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.ImageUrl).IsRequired().HasMaxLength(200).IsUnicode(false);
        builder.HasMany(c => c.Comments).WithOne().HasForeignKey(c => c.CourseId).OnDelete(DeleteBehavior.Cascade);

    }
}
