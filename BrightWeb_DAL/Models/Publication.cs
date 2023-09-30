using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Models
{
    public class Publication
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public  string? Description { get; set; }
        //public bool IsFree { get; set; }
        public string FileUrl { get; set; } = "NotFound";
        public double Price { get; set; }
        public string ImageUrl { get; set; } = "NotFound";
    }
}
