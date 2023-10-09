using BrightWeb_BAL.DTO;
using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
    public interface IStudentRepository
    {
        void UpdateStudent(Student student);
        void DeleteStudent(Student student);
        Task<StudentDto?> GetStudentByIdAsync(string studentId, bool trackChanges);
        Task<Student?> GetSingleStudentByIdAsync(string studentId, bool trackChanges);
        Task<IEnumerable<StudentDto?>> GetAllStudentsAsync(bool trackChanges);
        Task<IEnumerable<StudentDto>> GetAllStudentsEnrolledInCourseAsync(Guid courseId, bool trackChanges);
        Task EnrollInCourse(EnrollmentForCreateDto enrollmentDto);
        Task<bool> CheckToEnroll(Guid courseId, string studentId);
        Task<Course> GetCourseByIdToCheck(Guid courseId);
        Task<List<EnrollmentDto>> GetEnrollementsToStudent(string studentId);
    }
}
