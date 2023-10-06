using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using SS_EDUP.Core.DTO_s;
using SS_EDUP.Core.Entities;
using SS_EDUP.Core.Entities.Specifications;
using SS_EDUP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SS_EDUP.Core.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepository<Course> _courseRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public CoursesService(IRepository<Course> courseRepo,
            IRepository<Category> categoryRepo,
            IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _courseRepo = courseRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        public async Task Create(CourseDto courseDto)
        {
            if(courseDto.File.Count > 0)
            {
                string webPathRoot = _webHostEnvironment.WebRootPath;
                var files = courseDto.File;
                string upload = webPathRoot + _configuration.GetValue<string>("ImageSettings:ImagePath");
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                courseDto.ImagePath = fileName + extension;
            }
            else
            {
                courseDto.ImagePath = "https://www.freeiconspng.com/thumbs/no-image-icon/no-image-icon-6.png";
            }
            

            await _courseRepo.Insert(_mapper.Map<Course>(courseDto));
            await _courseRepo.Save();
        }

        public async Task Delete(int id)
        {
            var course = await Get(id);
            if (course != null)
            {
                if (course.ImagePath != "https://www.freeiconspng.com/thumbs/no-image-icon/no-image-icon-6.png" && course.ImagePath != null)
                {
                    string upload = _webHostEnvironment.WebRootPath + _configuration.GetValue<string>("ImageSettings:ImagePath");
                    var oldFile = Path.Combine(upload, course.ImagePath);

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                }

                await _courseRepo.Delete(id);
                await _courseRepo.Save();
            }
        }

        public async Task<CourseDto?> Get(int id)
        {
            if (id < 0)
                return null;
            var course = await _courseRepo.GetByID(id);
            return _mapper.Map<CourseDto>(course);
        }

        public async Task<List<CourseDetailDto>> Get(params int[] ids)
        {
            var result = await _courseRepo.GetListBySpec(new Courses.ByIds(ids));
            return _mapper.Map<List<CourseDetailDto>>(result);
        }


        public async Task<List<CourseDetailDto>> GetAll()
        {
            //return  _mapper.Map<List<CourseDto>>(await _courseRepo.Get(includeProperties: "Category")) ;
            var result = await _courseRepo.GetListBySpec(new Courses.All());
            return _mapper.Map<List<CourseDetailDto>>(result);
        }


        public async Task Update(CourseDto courseDto)
        {
            var course = await Get(courseDto.Id);
            if(course != null)
            {
               
                    if (course.ImagePath != "https://www.freeiconspng.com/thumbs/no-image-icon/no-image-icon-6.png" && course.ImagePath != null)
                {
                    string upload = _webHostEnvironment.WebRootPath + _configuration.GetValue<string>("ImageSettings:ImagePath");
                    var oldFile = Path.Combine(upload, course.ImagePath);

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                }
                if (courseDto.File.Count > 0)
                {
                    string webPathRoot = _webHostEnvironment.WebRootPath;
                    var files = courseDto.File;
                    string uploads = webPathRoot + _configuration.GetValue<string>("ImageSettings:ImagePath");
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    courseDto.ImagePath = fileName + extension;
                }
                else
                {
                    courseDto.ImagePath = "https://www.freeiconspng.com/thumbs/no-image-icon/no-image-icon-6.png";

                }

            }
            
            await _courseRepo.Update(_mapper.Map<Course>(courseDto));
            await _courseRepo.Save();
        }

        public async Task<List<CourseDetailDto>> GetByCategory(int id)
        {
            var result = await _courseRepo.GetListBySpec(new Courses.ByCategory(id));
            return _mapper.Map<List<CourseDetailDto>>(result);
        }

        public async Task<List<CourseDetailDto>> GetByAuthor(string id)
        {
            var result = await _courseRepo.GetListBySpec(new Courses.ByAuthor(id));
            return _mapper.Map<List<CourseDetailDto>>(result);
        }
       
    }
}

