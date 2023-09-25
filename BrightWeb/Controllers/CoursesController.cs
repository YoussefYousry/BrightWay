using AutoMapper;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace BrightWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public CoursesController(
                 IRepositoryManager repository, IMapper mapper)
        {
            _repositoryManager = repository;
            _mapper = mapper;
        }
        [HttpGet]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _repositoryManager.Course.GetAllCoursesAsync();
            if (courses is null)
            {
                return NotFound("There are no courses in the database");
            }
            var courseDto = _mapper.Map<IEnumerable<CourseDto>>(courses);
            foreach (var course in courseDto)
            {
                if (course.HasDiscount)
                {
                    course.Price= await _repositoryManager.Course.CalculateFinalPrice(course.Id);
                }
            }
            return Ok(courseDto);
        }
        [HttpGet("Discount")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllDiscountedCourses()
        {
            var courses = await _repositoryManager.Course.GetDiscountCourses();
            if (courses is null)
                return NotFound("There are no discounted courses in the database");
            var courseDto = _mapper.Map<IEnumerable<CourseDto>>(courses);
            foreach (var course in courseDto)
            {
                if (course.HasDiscount)
                    course.Price = await _repositoryManager.Course.CalculateFinalPrice(course.Id);
            }
            return Ok(courseDto);
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseForCreationDto course)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var courseEntity = _mapper.Map<Course>(course);
            _repositoryManager.Course.CreateCourse(courseEntity);
            await _repositoryManager.SaveChangesAsync();
            return Ok(
                new { CourseId = courseEntity.Id }
                );
        }
        [HttpGet("{courseId}")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetCourse(Guid courseId)
        {
            var course = await _repositoryManager.Course.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            var courseDto = _mapper.Map<CourseDto>(course);
            if (courseDto.HasDiscount)
                courseDto.Price = await _repositoryManager.Course.CalculateFinalPrice(course.Id);
            return Ok(courseDto);
        }
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var course = await _repositoryManager.Course.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            _repositoryManager.Course.DeleteCourse(course);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{courseId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseForUpdateDto course
            , Guid courseId)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var courseEntity = await _repositoryManager.Course.GetCourseByIdAsync(courseId, trackChanges: true);
            if (courseEntity is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            _mapper.Map(course, courseEntity);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();

        }
        [HttpGet("Students/{courseId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStudentsEnrolledInCourse(Guid courseId)
        {
            var course = await _repositoryManager.Course.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            var enrolledStudents = await _repositoryManager.Student.GetAllStudentsEnrolledInCourseAsync(courseId, trackChanges: false);
            //var studentDto = _mapper.Map<IEnumerable<StudentDto>>(student);
            return Ok(enrolledStudents);
        }
        [HttpGet("CheckEnrollment/{courseId}/{studentId}")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> CheckEnrollement(Guid courseId, string studentId)
        {
            var student = await _repositoryManager.Student.GetStudentByIdAsync(studentId, trackChanges: false);
            var course = await _repositoryManager.Course.GetCourseByIdAsync(courseId, trackChanges: false);

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
        [HttpPut("Enrollment/{courseId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnrollForACourse([FromBody] EnrollmentDto enrollment, Guid courseId)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var student = await _repositoryManager.Student.GetSingleStudentByIdAsync(enrollment.studentId, trackChanges: true);
            var course = await _repositoryManager.Course.GetCourseByIdAsync(courseId, trackChanges: true);

            if (student is null || course is null)
            {
                return BadRequest($"Student's ID (OR) Course's ID doesn't exist in the database");

            }
            var enrolled = await _repositoryManager.Student.CheckToEnroll(courseId, enrollment.studentId);
            if (enrolled != false)
            {
                return BadRequest("Student is already enrolled in the course");
            }
            _repositoryManager.Student.EnrollForCourse(courseId, student);
            _mapper.Map(enrollment, course);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();

        }
    }

}
