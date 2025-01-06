using CourseStoreMinimalAPI.ApplicationServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.DAL.Configuration;
public class CourseCategoryConfig : IEntityTypeConfiguration<CourseCategory>
{
    public void Configure(EntityTypeBuilder<CourseCategory> builder)
    {
        builder.HasKey(c => new { c.CourseId, c.CategoryId });
    }
}
