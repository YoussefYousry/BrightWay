using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Models
{
    public class OnlineCourse : Course
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Link { get; set; }

    }
}
