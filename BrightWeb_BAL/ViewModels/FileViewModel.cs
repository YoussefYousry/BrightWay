using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.ViewModels
{
    public class FileViewModel
    {
        public FileStream File { get; set; }
        public TypeOfFile TypeOfFile { get; set; }
    }
}
