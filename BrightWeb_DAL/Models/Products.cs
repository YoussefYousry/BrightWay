using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public required string FileUrl { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Student> Students { get; set; }
        public Product() {
            Students = new HashSet<Student>();
        }

    }
}
