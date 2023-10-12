using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.DTO
{
    public class OnDemandCourseForUpdateDto
    {
      //  public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
      //  public string? Objectives { get; set; }
        public required double Price { get; set; }
        public double Discount { get; set; }
        public bool HasDiscount { get; set; }
        public required int Enrollments { get; set; }
        public required string Hours { get; set; }
        public string? IntructorName { get; set; }
        public string? IntructorDescription { get; set; }
    }
}
