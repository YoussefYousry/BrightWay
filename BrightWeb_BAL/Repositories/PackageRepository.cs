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
    public class PackageRepository : RepositoryBase<Package>, IPackageRepository
    {
        private AppDbContext _appDbContext { get; set; }
        private IMapper _mapper { get; set; }
        public PackageRepository(AppDbContext context,IMapper mapper) : base(context)
        {
            _appDbContext = context;
            _mapper = mapper;
        }
        public void CreatePackage(PackageForCreateDto packageDto)
        {
            var package = _mapper.Map<Package>(packageDto);
            Create(package);
        }
        public void DeletePackage(Package package)
        => Delete(package);
        public async Task<Package?> GetPackageByIdAsync(Guid packgeId,bool trackChanges)
          => await FindByCondition(p => p.Id == packgeId,trackChanges).FirstOrDefaultAsync();
        public async Task <List<PackageDto>> GetPackagesByCourseId(Guid courseId)
          => await FindByCondition(p=>p.CourseId == courseId,false)
            .ProjectTo<PackageDto>(_mapper.ConfigurationProvider).ToListAsync();

    }
}
