using CourseStoreMinimalAPI.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.Entities;
public sealed class Category : BaseEntity
{

    public string Name { get; set; } = "";
    public List<CourseCategory> CourseCategories { get; set; }
}
