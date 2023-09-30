using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Models
{
    public class OnDemandCourse : Course
    {
        public OnDemandCourse()
        {
            Sections = new HashSet<Section>();
            Packages = new HashSet<Package>();
        }
        public virtual ICollection<Section> Sections { get; set; }
        public ICollection<Package> Packages { get; set; }
    }
}
