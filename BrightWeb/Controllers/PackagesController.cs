using AutoMapper;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrightWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private IRepositoryManager _repositoryManager;
        private IMapper _mapper;
        public PackagesController (IRepositoryManager repositoryManager,IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePackage([FromBody] PackageForCreateDto packageDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
             _repositoryManager.Packages.CreatePackage(packageDto);
            await _repositoryManager.SaveChangesAsync();
            return StatusCode(201);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid packageId)
        {
            var package = await _repositoryManager.Packages.GetPackageByIdAsync(packageId,trackChanges:true);
            if(package is null)
            {
                return NotFound("Package Not Found");
            }
            _repositoryManager.Packages.DeletePackage(package);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("{packageId}")]
        public async Task<IActionResult> GetPackage(Guid packageId)
        {
            var package = await _repositoryManager.Packages.GetPackageByIdAsync(packageId,false);
            if(package is null)
            {
                return NotFound("Package Not Found");
            }
            var packageDto = _mapper.Map<PackageDto>(package);
            return Ok(packageDto);
        }
        [HttpGet("PackegesToCourse/{courseId}")]
        public async Task<IActionResult> GetPackagesToCourse(Guid courseId)
        {
             var course = await _repositoryManager.OnDemandCourse.GetCourseByIdAsync(courseId,trackChanges:false);
            if(course is null)
            {
                return NotFound("Course Id Not Found");
            }
            var packages = await _repositoryManager.Packages.GetPackagesByCourseId(courseId);
            return Ok(packages);
        }
        [HttpPut("{packageId}")]
        public async Task<IActionResult> UpdatePackage(Guid packageId, [FromBody] PackageForUpdateDto packagedto)
        {
            var package = await _repositoryManager.Packages.GetPackageByIdAsync(packageId, false);
            if (package is null)
            {
                return NotFound("Package Not Found");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _mapper.Map<Package>(packagedto);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
    }
}
