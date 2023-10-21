using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrightWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private IProjectsRepository _projectsRepository;
        public ProjectsController(IProjectsRepository projectsRepository) {
            _projectsRepository = projectsRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectsRepository.GetProjects();
            return Ok(projects);
        }
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProject(int projectId)
        {
            var project = await _projectsRepository.GetProject(projectId);
            return Ok(project);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectForCreateViewModel projectForCreateView)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _projectsRepository.CreateProject(projectForCreateView);
            return StatusCode(201);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectForCreateViewModel projectForCreateView)
        {
            await _projectsRepository.Update(projectForCreateView);
            return NoContent();
        }

    }
}
