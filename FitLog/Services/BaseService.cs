using FitLog.Models;
using FitLog.Models.DatabaseEntities;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseTable
    {
        protected readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public BaseService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
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

        public virtual Task<int> UpdateAsync(int id, T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<int> DeleteAsync(int id) 
        { 
            return DeleteAsync([id]);
        }

        public virtual async Task<int> DeleteAsync(int[] ids)
        {
            var entities = await GetAsQueryable()
                .Where(x => ids.Contains(x.Id) && x.CreatedByUserId == _currentUserService.UserId && !x.IsDeleted)
                .ToListAsync();

            foreach (var entity in entities)
            {
                entity.SetupUpdatedFields(_currentUserService.UserId);
                entity.IsDeleted = true;
            }

            return await _context.SaveChangesAsync();
        }
    }
}
