using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.DTO
{
    public class ProductForCreateDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
    }
}
