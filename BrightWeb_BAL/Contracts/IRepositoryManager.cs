﻿using System;
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
        Task SaveChangesAsync();
    }
}
