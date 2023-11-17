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
        public async Task AddVideoToSection(VideoViewModel videoVM )
        {
            var video = new Video
            {
                Title = videoVM.Title,
                SectionId = videoVM.SectionId,
                VideoUrl = videoVM.VideoUrl,
            };
            await _appDbContext.Videos.AddAsync(video);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<List<VideoViewModel>> GetVideosToSection(Guid sectionId)
        {
            return await _appDbContext.Videos.AsNoTracking().Where(c=>c.SectionId == sectionId).Select(c=>new VideoViewModel
            {
                Title=c.Title,
                SectionId = c.SectionId,
                Id = c.Id,
                VideoUrl = c.VideoUrl,
               
            }).ToListAsync();
        }
           public async Task<VideoViewModel?> GetSingleVideoToSection(int videoId)
            {
                return await _appDbContext.Videos.AsNoTracking().Where(c=>c.Id == videoId).Select(c=>new VideoViewModel
                {
                    Title=c.Title,
                    SectionId = c.SectionId,
                    Id = c.Id,
                    VideoUrl = c.VideoUrl,
               
                }).FirstOrDefaultAsync();
            }
        public async Task DeleteVideo(int videoId)
        {
            var video = await _appDbContext.Videos.FirstOrDefaultAsync(v => v.Id == videoId);
            _appDbContext.Videos.Remove(video);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task UpdateVideo(VideoViewModel videoVM)
        {
			var video = await _appDbContext.Videos.FirstOrDefaultAsync(v => v.Id == videoVM.Id);
            video.VideoUrl = videoVM.VideoUrl;
            video.Title = videoVM.Title;
            video.SectionId = videoVM.SectionId;
            await _appDbContext.SaveChangesAsync();
		}

    }
}
