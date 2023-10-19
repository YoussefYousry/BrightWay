using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.ViewModels
{
    public class ProjectForCreateViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<ProjectImageForCreate> AllSubImages { get; set; }
        public ProjectForCreateViewModel() { 
            AllSubImages = new List<ProjectImageForCreate>();
        }
    }
    public class ProjectImageForCreate
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool IsMainImage { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }
}
