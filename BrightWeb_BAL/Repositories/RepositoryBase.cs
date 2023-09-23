using BrightWeb_BAL.Contracts;
using BrightWeb_DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class 
    {
        protected AppDbContext _context;
        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<T> FindAll(bool trackChanges)
            => trackChanges?
            _context.Set<T>() : _context.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) 
            => !trackChanges ?
            _context.Set<T>().Where(expression).AsNoTracking()
            : _context.Set<T>().Where(expression).AsTracking(QueryTrackingBehavior.TrackAll);
        public void Create(T entity) => _context.Set<T>().Add(entity);
        public void Delete(T entity) => _context.Set<T>().Remove(entity);
        public void Update(T entity) => _context.Set<T>().Update(entity);
    }
}
