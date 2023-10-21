using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Extentions
{
    public static class StudentRepositoryExtention
    {
        public static IQueryable<Student> Search(this IQueryable<Student> students,string? searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string lowerCaseTerm = searchTerm.ToLower();
                students = students.Where(c => c.UserName!.ToLower().Contains(lowerCaseTerm) || c.Email!.ToLower().Contains(searchTerm));
                return students;
            }
            return students;
        }
    }
}
