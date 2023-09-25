using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.DTO
{
    public class CourseForCreationDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Objectives { get; set; }
        public required double Price { get; set; }
        public bool HasDiscount { get; set; }
        public double Discount { get; set; }
        public required string Hours { get; set; }
    }
}
