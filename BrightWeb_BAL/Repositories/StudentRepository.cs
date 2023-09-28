using AutoMapper;
using AutoMapper.QueryableExtensions;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
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
        public StudentRepository(AppDbContext context , IMapper mapper):base(context)
        {
               _mapper = mapper;
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

        public async Task<IEnumerable<StudentDto?>> GetAllStudentsAsync(bool trackChanges)
            => await FindAll(trackChanges: false)
            .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
            .OrderBy(e => e.FirstName)
            .ToListAsync();
        public async Task<IEnumerable<StudentDto>> GetAllStudentsEnrolledInCourseAsync(Guid courseId, bool trackChanges)
             => await FindByCondition(c => c.Courses
             .FirstOrDefault(c => c.Id == courseId)!
             .Id == courseId, trackChanges)
             .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
             .OrderBy(e => e.UserName)
             .ToListAsync();
        //public void EnrollForCourse(Guid courseId, Student student)
        //    => _context.Set<Course>().FirstOrDefault(c => c.Id == courseId)!.Students.Add(student);
        public void EnrollForCourse(Guid courseId, Student student)
        {
            var course = _context.Set<Course>().FirstOrDefault(c => c.Id == courseId);
            course!.Students.Add(student);
            course.Enrollments++;
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

    }
}
