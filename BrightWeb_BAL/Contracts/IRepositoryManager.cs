using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
    public interface IRepositoryManager
    {
        IStudentRepository Student{ get; }
        IOnlineCourseRepository OnlineCourse { get; }
        IOnDemandCoursesRepository OnDemandCourse{ get; }
        IProductsRepository Products { get; }
        IPublicationRepository Publications { get; }
        IPackageRepository Packages { get; }
        ISectionRepository Sections { get; }
        Task SaveChangesAsync();
    }
}
