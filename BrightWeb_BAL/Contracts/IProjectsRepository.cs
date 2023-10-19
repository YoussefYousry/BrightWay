using BrightWeb_BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
    public interface IProjectsRepository
    {
        Task CreateProject(ProjectForCreateViewModel projectForCreateView);
        Task<List<ProjectForCreateViewModel>> GetProjects();
        Task<ProjectForCreateViewModel?> GetProject(int projectId);
    }
}
