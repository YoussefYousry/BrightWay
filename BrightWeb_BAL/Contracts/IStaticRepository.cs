using BrightWeb_BAL.ViewModels;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
	public interface IStaticRepository
	{
		Task<List<HeaderViewModel>> GetHeaders();
		Task<Header> CreateHeader(Header header);
		Task UploadHeaderImage(int headerId, IFormFile file);
		Task DeleteHeader(int headerId);
		Task UploadPortfolio(IFormFile file);
		Task<FileStream?> GetPortofilo();
	}
}
