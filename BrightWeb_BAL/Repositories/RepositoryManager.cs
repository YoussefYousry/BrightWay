using AutoMapper;
using BrightWeb_BAL.Contracts;
using BrightWeb_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private IStudentRepository _student;
        private IOnlineCourseRepository _onlineCourse;
        private IOnDemandCoursesRepository _onDemandCourse;

        public RepositoryManager(AppDbContext context
            ,IMapper mapper
            ,IStudentRepository student
            ,IOnlineCourseRepository onlineCourse
            ,IOnDemandCoursesRepository onDemandCourse)
        {
            _context = context;
            _mapper = mapper;
            _student = student;
            _onlineCourse = onlineCourse;
            _onDemandCourse = onDemandCourse;
        }
        public IStudentRepository Student
        {
            get
            {
                _student ??= new StudentRepository(_context, _mapper);
                return _student;
            }
        }
        public IOnlineCourseRepository OnlineCourse
        {
            get
            {
                _onlineCourse ??= new OnlineCourseRepository(_context);
                return _onlineCourse;
            }
        }
        public IOnDemandCoursesRepository OnDemandCourse
        {
            get
            {
                _onDemandCourse ??= new OnDemandCoursesRepository(_context);
                return _onDemandCourse;
            }
        }
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
