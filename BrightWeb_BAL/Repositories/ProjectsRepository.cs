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
                    IsMainImage= s.IsMainImage,
                    

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
                    IsMainImage = s.IsMainImage,


                }).ToList(),

            }).FirstOrDefaultAsync();
        }
        public async Task Update(ProjectForCreateViewModel projectForCreateView)
        {

            using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var projectImages = _appDbContext.ProjectImages.Where(p=>p.Id == projectForCreateView.Id).Select(p=>p.ImageUrl);
                    foreach(var img in projectImages)
                    {
                         _filesManager.DeleteFile(img);
                    }
                    var result2 = await _appDbContext.Database.ExecuteSqlRawAsync($"delete from ProjectImages where ProjectId= {projectForCreateView.Id}");
                    var result = await _appDbContext.Database.ExecuteSqlRawAsync($"delete from Projects where Id= {projectForCreateView.Id}");
                   

                    // At this point, the SQL delete statements have been executed.

                    await CreateProject(projectForCreateView);

                    // If everything is successful, commit the transaction.
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // If an error occurs, roll back the transaction.
                    await transaction.RollbackAsync();
                    throw; // Rethrow the exception to handle it higher up the call stack.
                }
            }

        }
    }
}
