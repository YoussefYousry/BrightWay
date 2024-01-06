using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.ViewModels
{
	public class VideoViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string? VideoUrl { get; set; }
		public Guid SectionId { get; set; }
	}
}
