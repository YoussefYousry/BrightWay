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
        private readonly IFilesManager _filesManager;
        private IStudentRepository _student;
        private IOnlineCourseRepository _onlineCourse;
        private IOnDemandCoursesRepository _onDemandCourse;
        private IProductsRepository _Products;
        private IPublicationRepository _Publications;
        private IPackageRepository _Packages;
        private ISectionRepository _Section;
        public RepositoryManager(AppDbContext context
            ,IMapper mapper
            , IStudentRepository student
            , IOnlineCourseRepository onlineCourse
            , IOnDemandCoursesRepository onDemandCourse,
             IFilesManager filesManager,
             IProductsRepository products,
             IPublicationRepository publications,
             IPackageRepository packages,
             ISectionRepository section)
        {
            _context = context;
            _mapper = mapper;
            _student = student;
            _onlineCourse = onlineCourse;
            _onDemandCourse = onDemandCourse;
            _filesManager = filesManager;
            _Products = products;
            _Publications = publications;
            _Packages = packages;
            _Section = section;
        }
        public IStudentRepository Student
        {
            get
            {
                _student ??= new StudentRepository(_context, _mapper,_filesManager);
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
                _onDemandCourse ??= new OnDemandCoursesRepository(_context,_mapper,_filesManager);
                return _onDemandCourse;
            }
        }
        public IProductsRepository Products
        {
            get
            {
                _Products ??= new ProductsRepository(_context,_mapper,_filesManager);
                return _Products;
            }
        } 
        public IPublicationRepository Publications
        {
            get
            {
                _Publications ??= new PublicationRepository(_context,_mapper,_filesManager);
                return _Publications;
            }
        }
        public IPackageRepository Packages
        {
            get
            {
                _Packages ??= new PackageRepository(_context,_mapper);
                return _Packages;
            }
        }
        public ISectionRepository Sections
        {
            get
            {
                _Section ??= new SectionRepository(_context,_mapper);
                return _Section;
            }
        }
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
