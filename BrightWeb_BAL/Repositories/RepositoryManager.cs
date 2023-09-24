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

        public RepositoryManager(AppDbContext context
            ,IMapper mapper
            ,IStudentRepository student)
        {
            _context = context;
            _mapper = mapper;
            _student = student;
        }
        public IStudentRepository Student
        {
            get
            {
                _student ??= new StudentRepository(_context, _mapper);
                return _student;
            }
        }
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
