using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_DAL.Data;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Repositories
{
    public class OnlineCourseRepository : RepositoryBase<OnlineCourse> , IOnlineCourseRepository
    {
        private IFilesManager _filesManager;
        public OnlineCourseRepository(AppDbContext context, IFilesManager filesManager) : base(context)
        {
            _filesManager = filesManager;
        }

        public void CreateCourse(OnlineCourse course) => Create(course);
        public void UpdateCourse(OnlineCourse course) => Update(course);
        public void DeleteCourse(OnlineCourse course) => Delete(course);
        public async Task<OnlineCourse?> GetCourseByIdAsync(Guid courseId, bool trackChanges)
           => await FindByCondition(c => c.Id == courseId, trackChanges)
                        .FirstOrDefaultAsync();
        public async Task<IEnumerable<OnlineCourse?>> GetAllCoursesAsync()
           => await FindAll(trackChanges: false).ToListAsync();

        public async Task<IEnumerable<OnlineCourse?>> GetAllCoursesForStudentAsync(string studentId, bool trackChanges)
           => await FindByCondition(s => s.Students
                                    .FirstOrDefault(e => e.Id == studentId)!
                                    .Id == studentId, trackChanges)
                                    .ToListAsync();
        public async Task<IEnumerable<OnlineCourse?>> GetDiscountCourses()
            => await FindByCondition(d => d.HasDiscount == true, trackChanges: false)
            .ToListAsync();
        public async Task<double> CalculateFinalPrice(Guid courseId)
        {
            var course = await FindByCondition(c => c.Id == courseId, trackChanges: false).FirstOrDefaultAsync();
            if (course!.HasDiscount == true)
            {
                double discountAmount = (course.Discount / 100) * course.DefaultPrice;
                return course.DefaultPrice - discountAmount;
            }
            return course.DefaultPrice;
        }
        public async Task AddDiscount(DiscountDto discount)
        {
            await _context.Database
                          .ExecuteSqlRawAsync($"Update OnlineCourses SET HasDiscount = 1 , Discount = {discount.DiscountPercentage} WHERE Id = {discount.CourseId} ");
        }
        public async Task UploadImage(Guid courseId, IFormFile file)
        {
            var course = await _context.OnlineCourses.FirstOrDefaultAsync(p => p.Id == courseId);
            string url = _filesManager.UploadFiles(file);
            course!.ImageUrl = url;
        }
        public async Task UploadInstructorImage(Guid courseId, IFormFile file)
        {
            var course = await _context.OnlineCourses.FirstOrDefaultAsync(p => p.Id == courseId);
            string url = _filesManager.UploadFiles(file);
            course!.IntructorImageUrl = url;
        }
    }
}
