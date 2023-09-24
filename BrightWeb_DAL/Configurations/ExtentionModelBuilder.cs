using BrightWeb_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }
        public static void AddManyToManyTables(this ModelBuilder builder)
        {

        }
        public static void AddOneToManyRelationship(this ModelBuilder builder)
        {

        }
    }
}
