


namespace FitLog.Services
{
    public interface IBaseService<T> where T : class
    {
        Task<int> DeleteAsync(int id);
        Task<int> DeleteAsync(int[] ids);
        Task<List<T>> GetAllAsync();
        IQueryable<T> GetAsQueryable();
        Task<T?> GetAsync(int id);
        Task<int> UpdateAsync(int id, T entity);
    }
}
