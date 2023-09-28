using BrightWeb_BAL.Contracts;
using BrightWeb_DAL.Data;
using BrightWeb_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Repositories
{
    public class OnDemandCoursesRepository : RepositoryBase<OnDemandCourse> , IOnDemandCoursesRepository
    {
        public OnDemandCoursesRepository(AppDbContext context) : base(context)
        {
        }

        public void CreateCourse(OnDemandCourse course) => Create(course);
        public void UpdateCourse(OnDemandCourse course) => Update(course);
        public void DeleteCourse(OnDemandCourse course) => Delete(course);
        public async Task<OnDemandCourse?> GetCourseByIdAsync(Guid courseId, bool trackChanges)
           => await FindByCondition(c => c.Id == courseId, trackChanges)
                        .Include(s => s.Sections)
                        .FirstOrDefaultAsync();
        public async Task<IEnumerable<OnDemandCourse?>> GetAllCoursesAsync()
           => await FindAll(trackChanges: false).ToListAsync();

        public async Task<IEnumerable<OnDemandCourse?>> GetAllCoursesForStudentAsync(string studentId, bool trackChanges)
           => await FindByCondition(s => s.Students
                                    .FirstOrDefault(e => e.Id == studentId)!
                                    .Id == studentId, trackChanges)
                                    .ToListAsync();
        public async Task<IEnumerable<OnDemandCourse?>> GetDiscountCourses()
            => await FindByCondition(d => d.HasDiscount == true, trackChanges: false)
            .ToListAsync();
        public async Task<double> CalculateFinalPrice(Guid courseId)
        {
            var course = await FindByCondition(c => c.Id == courseId, trackChanges: false).FirstOrDefaultAsync();
            if (course!.HasDiscount == true)
            {
                double discountAmount = (course.Discount / 100) * course.Price;
                return course.Price - discountAmount;
            }
            return course.Price;
        }
    }
}
