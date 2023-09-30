using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace BrightWeb_DAL.Models
{
    public class Course
    {
        public Course()
        {
            Students = new HashSet<Student>(); 
        }
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Objectives { get; set; }
        public double DefaultPrice { get; set; }
        public double Discount { get; set; } = 0;
        public bool HasDiscount { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public required int Enrollments { get; set; }
        public required string Hours { get; set; }
        public string ImageUrl { get; set; } = "NotFound";
        public string? IntructorName { get; set; }
        public string? IntructorDescription { get; set; }
        public string? IntructorImageUrl { get; set; } = "NotFound"; 
       
    }
}
