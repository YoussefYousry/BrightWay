using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.ViewModels
{
    public class FileToUploadViewModel
    {
        public IFormFile File { get; set; }
        public string? Title { get; set; }
    }
}
