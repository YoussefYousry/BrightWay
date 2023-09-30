using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Models
{
    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<ProjectImages> AllSubImages { get; set; }
        public Project()
        {
            AllSubImages = new List<ProjectImages>();   
        }
    }
}
