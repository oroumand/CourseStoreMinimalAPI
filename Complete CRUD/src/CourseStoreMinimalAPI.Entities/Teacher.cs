using CourseStoreMinimalAPI.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.Entities;
public sealed class Teacher : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birtday { get; set; }
    public string ImageUrl { get; set; }
    public List<CourseTeacher> courseTeachers { get; set; }
}
