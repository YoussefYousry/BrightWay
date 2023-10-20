using BrightWeb_BAL.Contracts;
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
    public class ProjectsRepository : RepositoryBase<Project>, IProjectsRepository
    {
        public readonly AppDbContext _appDbContext;
        private IFilesManager _filesManager;
        public ProjectsRepository(AppDbContext context,IFilesManager filesManager) : base(context)
        {
            _appDbContext = context;
            _filesManager = filesManager;
        }
        public async Task CreateProject(ProjectForCreateViewModel projectForCreateView)
        {
            var project = new Project { Name = projectForCreateView.Name};
            foreach (var projImage in projectForCreateView.AllSubImages)
            {
                var subProjectImage = new ProjectImages
                {
                    ImageUrl = _filesManager.UploadFileByBytes(projImage.Image, projImage.Name),
                    IsMainImage = projImage.IsMainImage,
                    Description = projImage.Description,
                };
                project.AllSubImages.Add(subProjectImage);
            }
            await _appDbContext.Projects.AddAsync(project);
            await _appDbContext.SaveChangesAsync();

        }
        public async Task<List<ProjectForCreateViewModel>> GetProjects()
        {
            return await FindAll(false).Select(p=>new ProjectForCreateViewModel
            {
                Name = p.Name,
                AllSubImages = _appDbContext.ProjectImages.Where(c=>c.ProjectId == p.Id).Select(s=>new ProjectImageForCreate
                {
                    Id = s.Id,
                    Description = s.Description,
                    Image = _filesManager.GetFileBytes(s.ImageUrl)!,
                    IsMainImage= true,
                    

                }).ToList(),
                
            }).ToListAsync();
        }
        public async Task<ProjectForCreateViewModel?> GetProject(int projectId)
        {
            return await FindAll(false).Select(p => new ProjectForCreateViewModel
            {
                Name = p.Name,
                AllSubImages = _appDbContext.ProjectImages.Where(c => c.ProjectId == p.Id).Select(s => new ProjectImageForCreate
                {
                    Id = s.Id,
                    Description = s.Description,
                    Image = _filesManager.GetFileBytes(s.ImageUrl)!,
                    IsMainImage = true,


                }).ToList(),

            }).FirstOrDefaultAsync();
        }
    }
}
