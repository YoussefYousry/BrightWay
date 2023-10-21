using AutoMapper;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_BAL.RequestFeature;
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
        public async Task<IActionResult> GetAllStudents([FromQuery] StudentParamters paramters)
        {
            var student = await _repositoryManager.Student.GetAllStudentsAsync(paramters,trackChanges:false);
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
            var studentEntity = await _repositoryManager.Student.GetSingleStudentByIdAsync(studentId, trackChanges: true);
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
        //[HttpGet("Courses/{studentId}")]
        ////[Authorize(Roles = "Student,Admin")]
        //public async Task<IActionResult> GetStudentEnrolledCourses(string studentId)
        //{
        //    var student = await _repositoryManager.Student.GetStudentByIdAsync(studentId, trackChanges: false);
        //    if (student is null)
        //    {
        //        return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
        //    }
        //    var course = await _repositoryManager.Course.GetAllCoursesForStudentAsync(studentId, trackChanges: false);
        //    var courseDto = _mapper.Map<IEnumerable<OnlineCourseDto>>(course);
        //    return Ok(courseDto);

        //}
        [HttpGet("CheckEnrollment/{courseId}/{studentId}")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> CheckEnrollement(Guid courseId, string studentId)
        {
            var student = await _repositoryManager.Student.GetStudentByIdAsync(studentId, trackChanges: false);
            var course = await _repositoryManager.Student.GetCourseByIdToCheck(courseId);

            if (student is null || course is null)
            {
                return BadRequest($"Student's ID (OR) Course's ID doesn't exist in the database");

            }
            bool result = await _repositoryManager.Student.CheckToEnroll(courseId, studentId);
            return Ok(new
            {
                IsEnrolled = result,
            });
        }

        [HttpPost("Enrollment")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnrollForACourse([FromBody] EnrollmentForCreateDto enrollment)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var student = await _repositoryManager.Student.GetSingleStudentByIdAsync(enrollment.StudentId, trackChanges: true);
            var course = await _repositoryManager.Student.GetCourseByIdToCheck(enrollment.CourseId);

            if (student is null || course is null)
            {
                return BadRequest($"Student's ID (OR) Course's ID doesn't exist in the database");

            }
            var enrolled = await _repositoryManager.Student.CheckToEnroll(enrollment.CourseId, enrollment.StudentId);
            if (enrolled != false)
            {
                return BadRequest("Student is already enrolled in the course");
            }
          await  _repositoryManager.Student.EnrollInCourse(enrollment);
           // _mapper.Map(enrollment, course);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();

        }
        [HttpGet("Enrollements/{studentId}")]
        public async Task<IActionResult> GetEnrollmentsToStudent(string studentId)
        {
            var student = await _repositoryManager.Student.GetSingleStudentByIdAsync(studentId, trackChanges: false);
            if(student is null)
            {
                return NotFound("studentId not Found");
            }
            var enrollments = await _repositoryManager.Student.GetEnrollementsToStudent(studentId);
            return Ok(enrollments);
        }
        [HttpGet("Products/{studentId}")]
        public async Task<IActionResult> GetProductsToStudent(string studentId)
        {
            var student = await _repositoryManager.Student.GetSingleStudentByIdAsync(studentId, trackChanges: false);
            if (student is null)
            {
                return NotFound("studentId not Found");
            }
            var products = await _repositoryManager.Student.GetProductsByStudentId(studentId);
            return Ok(products);
        }
    }
}
