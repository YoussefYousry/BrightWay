using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.DTO
{
    public class PackageForUpdateDto
    {
        public string Name { get; set; }
        public int DurationByMonthes { get; set; }
        public double Price { get; set; }
    }
}
