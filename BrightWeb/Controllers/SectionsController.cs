using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrightWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private IRepositoryManager _repositoryManager;
        public SectionsController(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetSectionsToCourse(Guid courseId)
        {
            var course = await _repositoryManager.OnDemandCourse.GetCourseByIdAsync(courseId, false);
            if(course is null)
            {
                return NotFound();
            }
            var sections = await _repositoryManager.Sections.GetSectionsToCourse(courseId);
            return Ok(sections);

        }
        [HttpPost]
        public async Task<IActionResult> Create(SectionForCreateDto section)
        {
            if(!ModelState!.IsValid)
            {
                return BadRequest();
            }
             _repositoryManager.Sections.CreateSection(section);
            await _repositoryManager.SaveChangesAsync();
            return StatusCode(201);
        }
        [HttpPut("UpdateSectionName/{sectionId}")]
        public async Task<IActionResult> UpdateSectionName(Guid sectionId,[FromBody]string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return BadRequest("You should enter the name");
            }
           await _repositoryManager.Sections.UpdateSectionName(sectionId, name);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{sectionId}")]
        public async Task<IActionResult> Delete(Guid sectionId)
        {
            var section = await _repositoryManager.Sections.GetSection(sectionId,true);
            if(section is null)
            {
                return NotFound();
            }
            _repositoryManager.Sections.DeleteSection(section);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }        
    }
}
