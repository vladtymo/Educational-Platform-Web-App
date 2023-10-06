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
    public class AutoMapperCourseAndCategoryProfile : Profile
    {
        public AutoMapperCourseAndCategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Course, CourseDto>()
                .ForMember(
                    dst => dst.CategoryName,
                    act => act.MapFrom(x => GetCategoryName(x)));
   
            CreateMap<CourseDto, Course>();
            CreateMap<Course, CourseDetailDto>()
                .ForMember(
                    dst => dst.CategoryName,
                    act => act.MapFrom(x => GetCategoryName(x)))
            .ForMember(
            dst => dst.AuthorFullName,
            act => act.MapFrom(x => GetAuthorFullName(x)));
        }

        static string GetCategoryName(Course course)
        {
            return course.Category?.Name ?? "Not loaded";
        }
        static string GetAuthorFullName(Course course)
        {
            string? fullname = $"{course.Author?.Surname} {course.Author.Name}" ?? course.Author?.UserName;
            return fullname ?? "Not loaded";
        }

    }
}
