using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.ViewModels;
using BrightWeb_DAL.Data;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Repositories
{
	public class StaticRepository : IStaticRepository
	{
		private readonly AppDbContext _appDbContext;
		private readonly IFilesManager _filesManager;
		public StaticRepository(AppDbContext appDbContext, IFilesManager filesManager)
		{
			_appDbContext = appDbContext;
			_filesManager = filesManager;
		}
		public async Task<List<HeaderViewModel>> GetHeaders()
		{
			var headers = await _appDbContext.Headers.AsNoTracking().OrderBy(c=>c.Order).Select(h=> new HeaderViewModel
			{
				Id = h.Id,
				ImageBytes = _filesManager.GetFileBytes(h.ImageUrl)!,
				Order = h.Order,

			}).ToListAsync();
			return headers;
		}
		public async Task<Header> CreateHeader(Header header)
		{
			var result = await _appDbContext.Headers.AddAsync(header);
			await _appDbContext.SaveChangesAsync();
			return result.Entity;
		}
		public async Task UploadHeaderImage(int headerId, IFormFile file)
		{
			var header = await _appDbContext.Headers.FirstOrDefaultAsync(h => h.Id == headerId);
			string url = _filesManager.UploadFiles(file);
			header!.ImageUrl = url;
			await _appDbContext.SaveChangesAsync();
		}
		public async Task DeleteHeader(int headerId)
		{
			var header = await _appDbContext.Headers.FirstOrDefaultAsync(h=> h.Id == headerId);
			_filesManager.DeleteFile(header.ImageUrl);
			_appDbContext.Headers.Remove(header);
			await _appDbContext.SaveChangesAsync();

		}

		public async Task UploadPortfolio(IFormFile file)
		{
			var portfolio = await _appDbContext.Portfolios.FirstOrDefaultAsync();
			if(portfolio == null)
			{
				var result = await _appDbContext.Portfolios.AddAsync(new Portfolio
				{
					FileUrl = "",

				});
				await _appDbContext.SaveChangesAsync();
				portfolio = result.Entity;
			}
			string url = _filesManager.UploadFiles(file);
			portfolio!.FileUrl = url;
			await _appDbContext.SaveChangesAsync();
		}
		public async Task<FileStream?> GetPortofilo()
		{
			var portfolio = await _appDbContext.Portfolios.FirstOrDefaultAsync();
			var file = _filesManager.GetFile(portfolio!.FileUrl);
			return file;
		}

	}
}
