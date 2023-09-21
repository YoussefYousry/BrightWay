using BrightWeb_BAL.Contracts;
using BrightWeb_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;

        public RepositoryManager(AppDbContext context)
        {
                _context = context;
        }
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
