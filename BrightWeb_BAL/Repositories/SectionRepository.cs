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
    public class SectionRepository : RepositoryBase<Section>, ISectionRepository
    {
        private readonly AppDbContext _appDbContext;
        private IMapper _mapper;
        public SectionRepository(AppDbContext context,IMapper mapper) : base(context)
        {
            _appDbContext = context;
            _mapper = mapper;
        }
        public async Task<List<SectionDto>> GetSectionsToCourse(Guid courseId)
          => await FindByCondition(s => s.CourseId == courseId, false).ProjectTo<SectionDto>(_mapper.ConfigurationProvider).ToListAsync();
        public async Task<Section?> GetSection(Guid sectionId, bool trackChanges)
            => await FindByCondition(c => c.Id == sectionId, trackChanges).FirstOrDefaultAsync();
        public void CreateSection(SectionForCreateDto sectionDto)
        {
           Section section = _mapper.Map<Section>(sectionDto);
            Create(section);
        }
        public void DeleteSection(Section section)
          => Delete(section);
        public async Task UpdateSectionName(Guid sectionId,string newName)
        {
          Section? section =  await GetSection(sectionId, true);
            section!.Name = newName;

        }


    }
}
