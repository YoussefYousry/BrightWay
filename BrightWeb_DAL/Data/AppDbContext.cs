using BrightWeb_DAL.Configurations;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<OnlineCourse> OnlineCourses { get; set; }
        public DbSet<OnDemandCourse> OnDemandCourses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectImages> ProjectImages { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Header> Headers { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
          //  builder.ApplyConfiguration(new RoleConfigrations());
            builder.AddIndexes();
            builder.AddInhertanceTables();
            builder.AddManyToManyTables();
            builder.AddOneToManyRelationship();
        }
    }
}
