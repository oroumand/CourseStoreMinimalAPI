using CourseStoreMinimalAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.ApplicationServices;
public class CourseTeacher
{
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public Course Course { get; set; }
    public Teacher Teacher { get; set; }
    public int SortOrder { get; set; }
}
public class CourseCategory
{
    public int CourseId { get; set; }
    public int CategoryId { get; set; }
    public Course Course { get; set; }
    public Category Category { get; set; }
    public int SortOrder { get; set; }

}