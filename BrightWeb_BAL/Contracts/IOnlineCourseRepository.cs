using BrightWeb_DAL.Models;
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
    }
}
