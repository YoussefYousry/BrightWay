using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.DTO
{
    public class EnrollmentDto
    {
        public Guid Id { get; set; }
       
        public  string StudentId { get; set; }
        public StudentDto? Student { get; set; }
        public Guid CourseId { get; set; }
        public OnDemandCourseDto? Course { get; set; }
        public Guid? PackageId { get; set; }
        public PackageDto? Package { get; set; }
        public DateTime StartDate { get; set; }
    }
}
