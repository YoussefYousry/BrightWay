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
    public class CourseRepository : RepositoryBase<Course> , ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context)
        {
        }

        public void CreateCourse(Course course) => Create(course);
        public void UpdateCourse(Course course) => Update(course);
        public void DeleteCourse(Course course) => Delete(course);
        public async Task<Course?> GetCourseByIdAsync(Guid courseId, bool trackChanges)
           => await FindByCondition(c => c.Id == courseId, trackChanges)
                        .FirstOrDefaultAsync();
        public async Task<IEnumerable<Course?>> GetAllCoursesAsync()
           => await FindAll(trackChanges: false).ToListAsync();

        public async Task<IEnumerable<Course?>> GetAllCoursesForStudentAsync(string studentId, bool trackChanges)
           => await FindByCondition(s => s.Students
                                    .FirstOrDefault(e => e.Id == studentId)!
                                    .Id == studentId, trackChanges)
                                    .ToListAsync();
        public async Task<IEnumerable<Course?>>GetDiscountCourses()
            => await FindByCondition(d => d.HasDiscount == true , trackChanges:false)
            .ToListAsync() ;
        public async Task<double> CalculateFinalPrice(Guid courseId)
        {
            var course = await FindByCondition(c => c.Id == courseId , trackChanges:false).FirstOrDefaultAsync();
            if (course!.HasDiscount==true)
            {
                double discountAmount = (course.Discount / 100) * course.Price;
                return course.Price - discountAmount;
            }
                return course.Price;
        }
    }
}
