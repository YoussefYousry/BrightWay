using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Models
{
    public class Video
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? VideoUrl { get; set; }

        [ForeignKey(nameof(Section))]
        public Guid SectionId { get; set; }
        public virtual Section? Section { get; set; }
    }
}
