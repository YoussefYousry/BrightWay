using BrightWeb_BAL.DTO;
using BrightWeb_BAL.ViewModels;
using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
    public interface IOnDemandCoursesRepository
    {
        void CreateCourse(OnDemandCourse course);
        void UpdateCourse(OnDemandCourse course);
        void DeleteCourse(OnDemandCourse course);
        Task<OnDemandCourse?> GetCourseByIdAsync(Guid courseId, bool trackChanges);
        Task<IEnumerable<OnDemandCourse?>> GetAllCoursesAsync();
        Task<IEnumerable<OnDemandCourse?>> GetAllCoursesForStudentAsync(string studentId, bool trackChanges);
        Task<IEnumerable<OnDemandCourse?>> GetDiscountCourses();
        Task<double> CalculateFinalPrice(Guid courseId);
        Task<OnDemandCourseViewModel?> GetCourse(Guid courseId);
        Task<List<OnDemandCourseViewModel>> GetCourses();
        Task AddDiscount(DiscountDto discount);
    }
}
