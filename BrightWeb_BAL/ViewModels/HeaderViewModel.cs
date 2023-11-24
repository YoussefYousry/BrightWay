using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.ViewModels
{
	public class HeaderViewModel
	{
		public int Id { get; set; }
		public byte[]? ImageBytes { get; set; }
		public int Order { get; set; }
	}
}
