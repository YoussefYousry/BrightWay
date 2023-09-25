using AutoMapper;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace BrightWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class StudentsController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public StudentsController(
            IRepositoryManager repository,IMapper mapper)
        {
            _repositoryManager = repository;
            _mapper = mapper;
        }
        [HttpGet]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllStudents()
        {
            var student = await _repositoryManager.Student.GetAllStudentsAsync(trackChanges:false);
            if (student is null)
            {
                return NotFound("There are no students in the database");
            }
            var studentDto = _mapper.Map<IEnumerable<StudentDto>>(student);
            return Ok(studentDto);
        }
        [HttpGet("{studentId}")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetStudent(string studentId)
        {
            var student = await _repositoryManager.Student.GetStudentByIdAsync(studentId, trackChanges: false);
            if (student is null)
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }
        [HttpPut("{studentId}")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentForUpdateDto student,
            string studentId)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var studentEntity = await _repositoryManager.Student.GetStudentByIdAsync(studentId, trackChanges: true);
            if (studentEntity is null)
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            _mapper.Map(student, studentEntity);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();

        }
        [HttpDelete("{studentId}")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> DeleteStudent(string studentId)
        {
            var student = await _repositoryManager.Student.GetSingleStudentByIdAsync(studentId, trackChanges: false);
            if (student is null)
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            _repositoryManager.Student.DeleteStudent(student);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("Courses/{studentId}")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetStudentEnrolledCourses(string studentId)
        {
            var student = await _repositoryManager.Student.GetStudentByIdAsync(studentId, trackChanges: false);
            if (student is null)
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            var course = await _repositoryManager.Course.GetAllCoursesForStudentAsync(studentId, trackChanges: false);
            var courseDto = _mapper.Map<IEnumerable<CourseDto>>(course);
            return Ok(courseDto);

        }
    }
}
