using FitLog.Models;
using FitLog.Models.DatabaseEntities;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseTable
    {
        protected readonly ApplicationDbContext _context;

        public BaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual IQueryable<T> GetAsQueryable()
        {
            return _context.Set<T>().Where(x => !x.IsDeleted);
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return GetAsQueryable().ToListAsync();
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await GetAsQueryable().SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
