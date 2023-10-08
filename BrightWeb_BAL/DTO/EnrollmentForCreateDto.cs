using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.DTO
{
    public class EnrollmentForCreateDto
    {
      //  public Guid Id { get; set; }
        public required string StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Guid? PackageId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
