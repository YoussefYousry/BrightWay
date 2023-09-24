using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightWeb_DAL.Models
{
    public class Student : User
    {
        public Student()
        {
            Courses = new HashSet<Course>();
        }
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public Gender Gender { get; set; }
    }
    public enum Gender
    {
        MALE,
        FEMALE
    }
}
