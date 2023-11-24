using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.ViewModels;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrightWeb.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StaticController : ControllerBase
	{
		private readonly IStaticRepository _staticRepository;
		public StaticController(IStaticRepository staticRepository)
		{
			_staticRepository = staticRepository;
		}
		[HttpGet("GetHeaders")]
		public async Task<IActionResult> GrtHeaders()
		{
			var headers = await _staticRepository.GetHeaders();
			return Ok(headers);
		}
		[HttpPost("CreateHeader")]
		public async Task<IActionResult> CreateHeader(Header header)
		{
			var result =await _staticRepository.CreateHeader(header);
			return Ok(result);
		}
		[HttpPost("UploadHeaderImage/{headerId}")]
		public async Task<IActionResult> UploadHeaderImage(int headerId,[FromForm] FileToUploadViewModel file)
		{
			await _staticRepository.UploadHeaderImage(headerId, file.File);
			return NoContent();
		}
		[HttpDelete("DeleteHeader")]
		public async Task<IActionResult> DeleteHeader(int headerId)
		{
			await _staticRepository.DeleteHeader(headerId);
			return NoContent();
		}
		[HttpPost("UploadPorotfolio")]
		public async Task<IActionResult> UploadPorotfolio([FromForm]FileToUploadViewModel file)
		{
			await _staticRepository.UploadPortfolio(file.File);
			return NoContent();
		}
		public async Task<IActionResult> GetPortofilo()
		{
			var result = await _staticRepository.GetPortofilo();
			if(result == null)
			{
				return NotFound();
			}
			return new FileStreamResult(result!, "application/pdf");
		}
	}
}
