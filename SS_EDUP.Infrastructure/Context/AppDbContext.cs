using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SS_EDUP.Core.DTO_s;
using SS_EDUP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Infrastructure.Context
{
    internal class AppDbContext : IdentityDbContext
    {
        public AppDbContext() : base() { }
        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.SeedCategories();
            modelBuilder.SeedCourses();

            //// Makes categories unique
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            //// Makes courses uniqe 
            modelBuilder.Entity<Course>().HasIndex(c => c.Title).IsUnique();

        }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Learning> Learnings { get; set; }
    }
}
