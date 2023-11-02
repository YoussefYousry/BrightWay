using AutoMapper;
using AutoMapper.QueryableExtensions;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_BAL.ViewModels;
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
        private IMapper _mapper;
        private IFilesManager _filesManager;
        public OnDemandCoursesRepository(AppDbContext context,IMapper mapper, IFilesManager filesManager) : base(context)
        {
            _filesManager = filesManager;
            _mapper = mapper;
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
                double discountAmount = (course.Discount / 100) * course.DefaultPrice;
                return course.DefaultPrice - discountAmount;
            }
            return course.DefaultPrice;
        }
        public async Task<OnDemandCourseViewModel?> GetCourse(Guid courseId)
        {
            var course = await FindByCondition(c => c.Id == courseId, false).Include(c => c.Packages)
                .ProjectTo<OnDemandCourseDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            var result = new OnDemandCourseViewModel
            {
                Name = course!.Name,
                Description = course.Description,
                DefaultPrice = course.Price,
                Discount = course.Discount,
                Enrollments = course.Enrollments,
                HasDiscount = course.HasDiscount,
                Hours = course.Hours,
                Id = course.Id,
                IntructorDescription = course.IntructorDescription,
                ImageBytes = _filesManager.GetFileBytes(course.ImageUrl),
                IntructorName = course.IntructorName,
                IntructorImageBytes = _filesManager.GetFileBytes(course.IntructorImageUrl!),
                Packages = course.Packages,
                Sections = course.Sections,
            };
            return result;    
        }
        public async Task<List<OnDemandCourseViewModel>> GetCourses()
         => await FindAll(false).Include(c => c.Packages)
                .ProjectTo<OnDemandCourseDto>(_mapper.ConfigurationProvider)
                .Select(course => new OnDemandCourseViewModel
                     {
                     Name = course!.Name,
                     Description = course.Description,
                     Discount = course.Discount,
                     Enrollments = course.Enrollments,
                     HasDiscount = course.HasDiscount,
                     Hours = course.Hours,
                     Id = course.Id,
                     IntructorDescription = course.IntructorDescription,
                     ImageBytes = _filesManager.GetFileBytes(course.ImageUrl),
                     IntructorName = course.IntructorName,
                     IntructorImageBytes = _filesManager.GetFileBytes(course.IntructorImageUrl!),
                    Packages = course.Packages,
                    Sections = course.Sections,
                }).ToListAsync();
        public async Task AddDiscount(DiscountDto discount)
        {
            await _context.Database
                          .ExecuteSqlRawAsync($"Update OnDemandCourses SET HasDiscount = 1 , Discount = {discount.DiscountPercentage} WHERE Id = {discount.CourseId} ");
        }
        
    }
}
