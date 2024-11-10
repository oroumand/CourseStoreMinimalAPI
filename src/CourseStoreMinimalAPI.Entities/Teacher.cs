using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.Entities
{
    public sealed class Teacher : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string? Picture { get; set; }

    }
}
