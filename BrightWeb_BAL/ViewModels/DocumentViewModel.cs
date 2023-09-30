using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.ViewModels
{
    public class DocumentViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public  byte[]? FileBytes { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public byte[]? ImageBytes { get; set; }
    }
}
