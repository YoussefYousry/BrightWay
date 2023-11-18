﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_BAL.Extentions;
using BrightWeb_BAL.RequestFeature;
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
    public class StudentRepository : RepositoryBase<Student> , IStudentRepository
    {
        private readonly IMapper _mapper;
        private readonly IFilesManager _filesManager;
        public StudentRepository(AppDbContext context , IMapper mapper, IFilesManager filesManager) : base(context)
        {
            _mapper = mapper;
            _filesManager = filesManager;
        }
        public void UpdateStudent(Student student) => Update(student);
        public void DeleteStudent(Student student) => Delete(student);
        public async Task<StudentDto?> GetStudentByIdAsync(string studentId, bool trackChanges)
            => await FindByCondition(e => e.Id == studentId, trackChanges)
            .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        public async Task<Student?> GetSingleStudentByIdAsync(string studentId, bool trackChanges)
            => await FindByCondition(e => e.Id == studentId, trackChanges)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<StudentDto?>> GetAllStudentsAsync(StudentParamters paramters,bool trackChanges)
            => await FindAll(trackChanges: false)
            .Search(paramters.SearchTerm)
            .Skip((paramters.PageNumber - 1) * paramters.PageSize)
            .Take(paramters.PageSize)
            .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
            .OrderBy(e => e.FirstName)
            .ToListAsync();
        public async Task<IEnumerable<StudentDto>> GetAllStudentsEnrolledInCourseAsync(Guid courseId, bool trackChanges)
             => await _context.Students.Include(s=>s.Courses).Where(c => c.Courses
             .FirstOrDefault(c => c.Id == courseId)!
             .Id == courseId)
             .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
             .OrderBy(e => e.UserName)
             .ToListAsync();
        //public void EnrollForCourse(Guid courseId, Student student)
        //    => _context.Set<Course>().FirstOrDefault(c => c.Id == courseId)!.Students.Add(student);
        private async Task AddEnrollment(Guid courseId)
        {
            var course = await _context.Set<Course>().FirstOrDefaultAsync(c => c.Id == courseId);
          //  course!.Students.Add(student);
            course!.Enrollments++;
        }
        public async Task<bool> CheckToEnroll(Guid courseId, string studentId)
        {
            var students = await GetAllStudentsEnrolledInCourseAsync(courseId, false);
            var student = students.Where(e => e.Id == studentId).FirstOrDefault();
            if (student is null)
            {
                return false;
            }
            return true;
        }
        public async Task<Course> GetCourseByIdToCheck(Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            return course!;
        }
      public async Task EnrollInCourse(EnrollmentForCreateDto enrollmentDto)
        {
            Enrollment enrollment = _mapper.Map<Enrollment>(enrollmentDto); 
           await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
            await AddEnrollment(enrollment.CourseId);
        }
        public async Task<List<EnrollmentDto>> GetEnrollementsToStudent(string studentId)
        {
            var enrollments = await _context.Enrollments
                .Include(e=>e.Student)
                .Include(c=>c.Course)
                .Include(c=>c.Package)
                .Where(s => s.StudentId == studentId)
                .AsNoTracking()
                .ProjectTo<EnrollmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return enrollments;

        }
         public async Task<List<DocumentViewModel>> GetProductsByStudentId(string studentId)
                  => await _context.Products
                                   .Include(c=>c.Students)
                                   .Where(p=>p.Students.Any(s=>s.Id == studentId))
                                   .Select(p => new DocumentViewModel
                                   {
                                       Description = p.Description,
                                       Title = p.Title,
                                       Id = p.Id,
                                       Price = p.Price,
                                       // FileBytes = _filesManager.GetFileBytes(p.FileUrl),
                                       ImageBytes = _filesManager.GetFileBytes(p.ImageUrl)
                                   })
                                   .ToListAsync();
        

    }
}
