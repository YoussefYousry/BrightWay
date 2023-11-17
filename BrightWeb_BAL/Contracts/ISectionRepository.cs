using BrightWeb_BAL.DTO;
using BrightWeb_BAL.ViewModels;
using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
    public interface ISectionRepository
    {
        Task<List<SectionDto>> GetSectionsToCourse(Guid courseId);
        Task<Section?> GetSection(Guid sectionId, bool trackChanges);
        void CreateSection(SectionForCreateDto sectionDto);
        void DeleteSection(Section section);
        Task UpdateSectionName(Guid sectionId, string newName);
        Task AddVideoToSection(VideoViewModel videoVM);
        Task<List<VideoViewModel>> GetVideosToSection(Guid sectionId);
        Task<VideoViewModel?> GetSingleVideoToSection(int videoId);
        Task DeleteVideo(int videoId);
        Task UpdateVideo(VideoViewModel videoVM);

	}
}
