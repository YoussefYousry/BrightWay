using AutoMapper;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_BAL.ViewModels;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrightWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnlineCoursesController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public OnlineCoursesController(
                 IRepositoryManager repository, IMapper mapper)
        {
            _repositoryManager = repository;
            _mapper = mapper;
        }
        [HttpGet]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _repositoryManager.OnlineCourse.GetAllCoursesAsync();
            if (courses is null)
            {
                return NotFound("There are no courses in the database");
            }
            var courseDto = _mapper.Map<IEnumerable<OnlineCourseDto>>(courses);
            //foreach (var course in courseDto)
            //{
            //    if (course.HasDiscount)
            //    {
            //        course.Price = await _repositoryManager.OnlineCourse.CalculateFinalPrice(course.Id);
            //    }
            //}
            return Ok(courseDto);
        }
        [HttpGet("Discount")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllDiscountedCourses()
        {
            var courses = await _repositoryManager.OnlineCourse.GetDiscountCourses();
            if (courses is null)
                return NotFound("There are no discounted courses in the database");
            var courseDto = _mapper.Map<IEnumerable<OnlineCourseDto>>(courses);
            //foreach (var course in courseDto)
            //{
            //    if (course.HasDiscount)
            //        course.Price = await _repositoryManager.OnlineCourse.CalculateFinalPrice(course.Id);
            //}
            return Ok(courseDto);
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCourse([FromBody] OnlineCourseForCreationDto course)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            if (course.Start > course.End)
                return BadRequest("Invalid Date Values");
            var courseEntity = _mapper.Map<OnlineCourse>(course);
            _repositoryManager.OnlineCourse.CreateCourse(courseEntity);
            await _repositoryManager.SaveChangesAsync();
            return Ok(
                new { CourseId = courseEntity.Id }
                );
        }
        [HttpGet("{courseId}")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetCourse(Guid courseId)
        {
            var course = await _repositoryManager.OnlineCourse.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            var courseDto = _mapper.Map<OnlineCourseDto>(course);
            //if (courseDto.HasDiscount)
            //    courseDto.Price = await _repositoryManager.OnlineCourse.CalculateFinalPrice(course.Id);
            return Ok(courseDto);
        }
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var course = await _repositoryManager.OnlineCourse.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            _repositoryManager.OnlineCourse.DeleteCourse(course);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{courseId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCourse([FromBody] OnlineCourseForUpdateDto course
            , Guid courseId)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var courseEntity = await _repositoryManager.OnlineCourse.GetCourseByIdAsync(courseId, trackChanges: true);
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
            var course = await _repositoryManager.OnlineCourse.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            var enrolledStudents = await _repositoryManager.Student.GetAllStudentsEnrolledInCourseAsync(courseId, trackChanges: false);
            //var studentDto = _mapper.Map<IEnumerable<StudentDto>>(student);
            return Ok(enrolledStudents);
        }

        [HttpPut("AddDiscount")]
        public async Task<IActionResult> AddDiscount([FromBody] DiscountDto discountDto)
        {
            await _repositoryManager.OnlineCourse.AddDiscount(discountDto);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("UploadCourseImage/{courseId}")]
        public async Task<IActionResult> UploadCourseImage(Guid courseId, [FromForm] FileToUploadViewModel fileVM)
        {
            try
            {
                await _repositoryManager.OnlineCourse.UploadImage(courseId, fileVM.File);
                await _repositoryManager.SaveChangesAsync();
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost("UploadInstructorImage/{courseId}")]
        public async Task<IActionResult> UploadInstructorImage(Guid courseId, [FromForm] FileToUploadViewModel fileVM)
        {
            try
            {
                await _repositoryManager.OnlineCourse.UploadInstructorImage(courseId, fileVM.File);
                await _repositoryManager.SaveChangesAsync();
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("GetAllOnlineCourses")]
        public async Task<IActionResult> GetAllOnDemandCourses()
        {
            var result = await _repositoryManager.OnlineCourse.GetCourses();
            return Ok(result);
        }
        [HttpGet("GetSingleCourse/{courseId}")]
        public async Task<IActionResult> GetSingleCourse(Guid courseId)
        {
            var result = await _repositoryManager.OnlineCourse.GetCourse(courseId);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
