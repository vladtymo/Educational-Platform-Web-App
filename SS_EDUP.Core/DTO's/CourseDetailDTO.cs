using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.DTO_s
{
    public class CourseDetailDto:CourseDto
    {
        public string? AuthorFullName { get; set; }

    }
}
