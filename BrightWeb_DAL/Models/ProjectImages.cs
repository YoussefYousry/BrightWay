using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Models
{
    public class ProjectImages
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool IsMainImage { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public required string ImageUrl { get; set; }
    }
}
