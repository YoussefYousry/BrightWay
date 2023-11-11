using BrightWeb_BAL.DTO;
using BrightWeb_BAL.ViewModels;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
    public interface IOnlineCourseRepository
    {
        void CreateCourse(OnlineCourse course);
        void UpdateCourse(OnlineCourse course);
        void DeleteCourse(OnlineCourse course);
        Task<OnlineCourse?> GetCourseByIdAsync(Guid courseId, bool trackChanges);
        Task<IEnumerable<OnlineCourse?>> GetAllCoursesAsync();
        Task<IEnumerable<OnlineCourse?>> GetAllCoursesForStudentAsync(string studentId, bool trackChanges);
        Task<IEnumerable<OnlineCourse?>> GetDiscountCourses();
        Task<double> CalculateFinalPrice(Guid courseId);
        Task AddDiscount(DiscountDto discount);
        Task UploadImage(Guid courseId, IFormFile file);
        Task UploadInstructorImage(Guid courseId, IFormFile file);
        Task<CourseViewModel?> GetCourse(Guid courseId);
        Task<List<CourseViewModel>> GetCourses();
    }
}
