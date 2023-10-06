using AutoMapper;
using SS_EDUP.Core.DTO_s;
using SS_EDUP.Core.Entities;
using SS_EDUP.Core.Entities.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.AutoMapper
{
    public class AutoMapperLearning: Profile
    {
        public AutoMapperLearning() {
            CreateMap<Learning, LearningDto>()
                    .ForMember(
                        dst => dst.CourseTitle,
                        act => act.MapFrom(x => GetCourseName(x)))
                     .ForMember(
                        dst => dst.CategoryName,
                        act => act.MapFrom(x => GetCategoryName(x)))
                     .ForMember(
                        dst => dst.ImagePath,
                        act => act.MapFrom(x => GetImagePath(x))) 
                     .ForMember(
                        dst => dst.AuthorName,
                        act => act.MapFrom(x => GetAuthorName(x)));
            CreateMap<LearningDto,Learning>();

        }

     
        static string GetCourseName(Learning learning)
        {
            return learning.Course?.Title ?? "Not loaded";
        }
        static string GetCategoryName(Learning learning)
        {
            return learning.Course?.Category?.Name ?? "Not loaded";
        }
        static string GetImagePath(Learning learning)
        {
           
            return learning.Course?.ImagePath;
        }
        static string GetAuthorName(Learning learning)
        {
            string? fullname = $"{learning.Course.Author?.Surname} {learning.Course.Author?.Name}" ?? learning.Course.Author?.UserName;
            return fullname ?? "Not loaded";
        }
    }
}
