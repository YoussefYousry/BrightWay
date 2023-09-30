using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Models
{
    public class Package
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DurationByMonthes { get; set; }
        public double Price { get; set; }
        [ForeignKey(nameof(Course))]
        public Guid CourseId { get; set; }
        public OnDemandCourse? Course { get; set;}
    }
}
