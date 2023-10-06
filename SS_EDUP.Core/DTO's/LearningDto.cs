using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.DTO_s
{
    public class LearningDto
    {
        public int Id { get; set; }
        public int CourseID { get; set; }
        //studentid
        public string AppUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Progress { get; set; }
        public string CourseTitle { get; set; }
        public string? CategoryName { get; set; }
        public string? ImagePath { get; set; }
        public string? AuthorName { get; set; }
       
    }
}

