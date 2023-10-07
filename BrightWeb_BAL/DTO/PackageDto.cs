using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.DTO
{
    public class PackageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DurationByMonthes { get; set; }
        public double Price { get; set; }
        public Guid CourseId { get; set; }
    }
}
