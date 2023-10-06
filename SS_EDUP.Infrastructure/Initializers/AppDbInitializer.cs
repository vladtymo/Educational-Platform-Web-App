using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SS_EDUP.Core.DTO_s;
using SS_EDUP.Core.Entities;
using SS_EDUP.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Infrastructure.Initializers
{
    public class AppDbInitializer
    {
        public static async Task SeedUsersAndRoles(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                UserManager<AppUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                if (userManager.FindByEmailAsync("admin@email.com").Result == null)
                {
                    AppUser admin = new AppUser()
                    {
                        UserName = "admin@email.com",
                        Email = "admin@email.com",
                        EmailConfirmed = true,
                        Name = "Admin",
                        Surname = "Admin",
                        PhoneNumber = "+xx(xxx)xxx-xx-xx",
                        PhoneNumberConfirmed = true,
                    };

                    AppUser teacher = new AppUser()
                    {
                        UserName = "teacher@email.com",
                        Email = "teacher@email.com",
                        EmailConfirmed = true,
                        Name = "Teacher",
                        Surname = "Teacher",
                        PhoneNumber = "+xx(xxx)xxx-xx-xx",
                        PhoneNumberConfirmed = true,
                    };

                    AppUser student = new AppUser()
                    {
                        UserName = "student@email.com",
                        Email = "student@email.com",
                        EmailConfirmed = true,
                        Name = "Student",
                        Surname = "Student",
                        PhoneNumber = "+xx(xxx)xxx-xx-xx",
                        PhoneNumberConfirmed = true,
                    };

                    context.Roles.AddRange(
                        new IdentityRole()
                        {
                            Name = "Administrators",
                            NormalizedName = "ADMINISTRATORS"
                        },
                        new IdentityRole()
                        {
                            Name = "Teachers",
                            NormalizedName = "TEACHERS"
                        },
                         new IdentityRole()
                         {
                             Name = "Students",
                             NormalizedName = "STUDENTS"
                         }
                    );

                    await context.SaveChangesAsync();

                    IdentityResult resultAdmin = userManager.CreateAsync(admin, "Qwerty-1").Result;
                    IdentityResult resulTeacher = userManager.CreateAsync(teacher, "Qwerty-1").Result;
                    IdentityResult resulStudent = userManager.CreateAsync(student, "Qwerty-1").Result;

                    if (resultAdmin.Succeeded)
                    {
                        userManager.AddToRoleAsync(admin, "Administrators").Wait();
                    }

                    if (resulTeacher.Succeeded)
                    {
                        userManager.AddToRoleAsync(teacher, "Teachers").Wait();
                    }

                    if (resulStudent.Succeeded)
                    {
                        userManager.AddToRoleAsync(student, "Students").Wait();
                    }
                }
            }

        }
    }
}
