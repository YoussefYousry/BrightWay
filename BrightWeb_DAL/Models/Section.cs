using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Models
{
    public class Section
    {
        public Section()
        {
                Videos = new HashSet<Video>();
        }
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<Video> Videos { get; set; }

        [ForeignKey(nameof(OnDemandCourse))]
        public Guid CourseId { get; set; }
        public virtual OnDemandCourse? OnDemandCourse { get; set; }    }
}
