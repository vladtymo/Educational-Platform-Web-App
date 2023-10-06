﻿using SS_EDUP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Entities
{
    public class Category:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        // ------------ Navigation Property
        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();

        public override string ToString()
        {
            return Name;
        }
    }
}
