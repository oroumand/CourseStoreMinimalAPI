using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.Entities;
public class Comment : BaseEntity
{
    public string CommentBody { get; set; }
    public DateTime CommentDate { get; set; }
    public int CourseId { get; set; }
}
