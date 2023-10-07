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
    public class OnDemandCoursesController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public OnDemandCoursesController(
                 IRepositoryManager repository, IMapper mapper)
        {
            _repositoryManager = repository;
            _mapper = mapper;
        }
        [HttpGet]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _repositoryManager.OnDemandCourse.GetAllCoursesAsync();
            if (courses is null)
            {
                return NotFound("There are no courses in the database");
            }
            var courseDto = _mapper.Map<IEnumerable<OnDemandCourseDto>>(courses);
            foreach (var course in courseDto)
            {
                if (course.HasDiscount)
                {
                    course.Price = await _repositoryManager.OnDemandCourse.CalculateFinalPrice(course.Id);
                }
            }
            return Ok(courseDto);
        }
        [HttpGet("Discount")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllDiscountedCourses()
        {
            var courses = await _repositoryManager.OnDemandCourse.GetDiscountCourses();
            if (courses is null)
                return NotFound("There are no discounted courses in the database");
            var courseDto = _mapper.Map<IEnumerable<OnDemandCourseDto>>(courses);
            foreach (var course in courseDto)
            {
                if (course.HasDiscount)
                    course.Price = await _repositoryManager.OnDemandCourse.CalculateFinalPrice(course.Id);
            }
            return Ok(courseDto);
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCourse([FromBody] OnDemandCourseForCreationDto course)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var courseEntity = _mapper.Map<OnDemandCourse>(course);
            _repositoryManager.OnDemandCourse.CreateCourse(courseEntity);
            await _repositoryManager.SaveChangesAsync();
            return Ok(
                new { CourseId = courseEntity.Id }
                );
        }
        [HttpGet("{courseId}")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetCourse(Guid courseId)
        {
            var course = await _repositoryManager.OnDemandCourse.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            var courseDto = _mapper.Map<OnDemandCourseDto>(course);
            if (courseDto.HasDiscount)
                courseDto.Price = await _repositoryManager.OnDemandCourse.CalculateFinalPrice(course.Id);
            return Ok(courseDto);
        }
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var course = await _repositoryManager.OnDemandCourse.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            _repositoryManager.OnDemandCourse.DeleteCourse(course);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{courseId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCourse([FromBody] OnDemandCourseForUpdateDto course
            , Guid courseId)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var courseEntity = await _repositoryManager.OnDemandCourse.GetCourseByIdAsync(courseId, trackChanges: true);
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
            var course = await _repositoryManager.OnDemandCourse.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            var enrolledStudents = await _repositoryManager.Student.GetAllStudentsEnrolledInCourseAsync(courseId, trackChanges: false);
            var studentDto = _mapper.Map<IEnumerable<StudentDto>>(enrolledStudents);
            return Ok(studentDto);
        }
        [HttpGet("GetAllOnDemandCourses")]
        public async Task<IActionResult> GetAllOnDemandCourses()
        {
            var result = await _repositoryManager.OnDemandCourse.GetCourses();
            return Ok(result);
        }
        [HttpGet("GetSingleCourse/{courseId}")]
        public async Task<IActionResult> GetSingleCourse(Guid courseId)
        {
            var result = await _repositoryManager.OnDemandCourse.GetCourse(courseId);
            if(result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }

}
