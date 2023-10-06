using SS_EDUP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Entities
{
    public class Course: IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; } = "https://www.freeiconspng.com/thumbs/no-image-icon/no-image-icon-6.png";
        public int CategoryId { get; set; }

        //author course foregin key roll teachers
        [ForeignKey("Author")]
        public string? AuthorId { get; set; }
        
        public Category? Category { get; set; }

     
        public AppUser? Author { get; set; }

        //lessons
        //public ICollection? Lessons { get; set; }
    }
}
