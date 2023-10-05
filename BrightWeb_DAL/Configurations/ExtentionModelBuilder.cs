using BrightWeb_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Configurations
{
    public static class ExtentionModelBuilder
    {
        public static void AddIndexes(this ModelBuilder builder)
        {

        }
        public static void AddInhertanceTables(this ModelBuilder builder)
        {
            builder.Entity<User>().UseTptMappingStrategy().ToTable("Users");
            builder.Entity<Student>()
                .ToTable("Students").HasBaseType<User>();
            builder.Entity<OnDemandCourse>()
                .ToTable("OnDemandCourses").HasBaseType<Course>();
            builder.Entity<OnlineCourse>()
                 .ToTable("OnlineCourses").HasBaseType<Course>();
        }
        public static void AddManyToManyTables(this ModelBuilder builder)
        {
            builder.Entity<Student>().HasMany(r => r.Courses).WithMany(c => c.Students);
            builder.Entity<Student>().HasMany(r => r.Products).WithMany(c => c.Students);
        }
        public static void AddOneToManyRelationship(this ModelBuilder builder)
        {
            builder.Entity<OnDemandCourse>().HasMany(c => c.Sections).WithOne(s => s.OnDemandCourse);
            builder.Entity<Section>().HasMany(s => s.Videos).WithOne(v => v.Section);
            builder.Entity<Product>()
           .HasIndex(p => p.Title);
            builder.Entity<Publication>()
                .HasIndex(p => p.Title);
        }
    }
}
