using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        //public ICollection<Course>? Courses { get; set; }
        public ICollection<Learning>? Learnings { get; set; }
    }
}
