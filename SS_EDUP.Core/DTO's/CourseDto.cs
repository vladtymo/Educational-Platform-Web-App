using Microsoft.AspNetCore.Http;
using SS_EDUP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace SS_EDUP.Core.DTO_s
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ? Description { get; set; }
        public decimal Price { get; set; }
        private string? _imagePath;
        public string? ImagePath
        {
            get => _imagePath;
            set => _imagePath =  value ?? defaultPath;
        }
        const string defaultPath = "https://www.freeiconspng.com/thumbs/no-image-icon/no-image-icon-6.png";// ??
        public int CategoryId { get; set; }
        public string ? CategoryName { get; set; }
        public string? AuthorId { get; set; }
    //  public string? AuthorFullName { get; set; }
        public IFormFileCollection File { get; set; }
    }
}
