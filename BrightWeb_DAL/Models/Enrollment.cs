using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Models
{
    public class Enrollment
    {
        public Guid Id { get; set; }
        [ForeignKey(nameof(Student))]
        public required string StudentId { get; set; }
        public Student? Student { get; set; }
        [ForeignKey(nameof(Course))]
        public Guid CourseId { get; set; }
        public OnDemandCourse? Course { get; set;}
        [ForeignKey(nameof(Package))]
        public Guid? PackageId { get; set; }
        public Package? Package { get; set; }
        public DateTime StartDate { get; set; }
    }
}
