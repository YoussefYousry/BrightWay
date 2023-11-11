using BrightWeb_BAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.ViewModels
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double DefaultPrice { get; set; }
        public double Discount { get; set; } = 0;
        public bool HasDiscount { get; set; }
        public  int Enrollments { get; set; }
        public  string Hours { get; set; }
        public byte[]? ImageBytes { get; set; }
        public string? IntructorName { get; set; }
        public string? IntructorDescription { get; set; }
        public byte[]? IntructorImageBytes { get; set; }
        public ICollection<PackageDto> Packages { get; set; } 
        public ICollection<SectionDto> Sections { get; set; }
    }
}
