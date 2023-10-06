using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SS_EDUP.Core.AutoMapper;
using SS_EDUP.Core.Entities;
using SS_EDUP.Core.Interfaces;
using SS_EDUP.Core.Services;
using SS_EDUP.Infrastructure.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core
{
    public static class ServiceExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            // Add user service
            services.AddTransient<UserService>();

            // Add email service
            services.AddTransient<EmailService>();

            // Add Course service
          //  services.AddTransient<CoursesService>();

            //// Add Category service
           // services.AddTransient<CategoriesService>();
            //// Add Learning service
           // services.AddTransient<LearningService>();

            // Add Fluent validation
            services.AddFluentValidation(x =>
            {
                x.DisableDataAnnotationsValidation = true;
                x.ImplicitlyValidateChildProperties = true;
                x.RegisterValidatorsFromAssemblyContaining<RegisterUserVM>();
            });

        }

        public static void AddMapping(this IServiceCollection services)
        {
            // Add user mapping
            services.AddAutoMapper(typeof(AutoMapperUserProfile));
            services.AddAutoMapper(typeof(AutoMapperCourseAndCategoryProfile));
            services.AddAutoMapper(typeof(AutoMapperLearning));
        }

        public static void AddCategoryAndCourseServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ICoursesService, CoursesService>();
            services.AddScoped<ILearningService, LearningService>();
        }



    }
}
