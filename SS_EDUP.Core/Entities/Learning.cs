using SS_EDUP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Entities
{
    public class Learning:IEntity
    {
        public int Id { get; set; }

        public int CourseID { get; set; }
        //studentid
        public string AppUserId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        [Range(0, 100)]
        public int Progress { get; set; }

        public Course? Course { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
