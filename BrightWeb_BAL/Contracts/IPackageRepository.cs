using BrightWeb_BAL.DTO;
using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
    public interface IPackageRepository
    {
        void CreatePackage(PackageForCreateDto packageDto);
        void DeletePackage(Package package);
        Task<Package?> GetPackageByIdAsync(Guid packgeId, bool trackChanges);
        Task<List<PackageDto>> GetPackagesByCourseId(Guid courseId);
    }
}
