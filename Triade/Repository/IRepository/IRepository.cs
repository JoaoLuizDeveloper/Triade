using System.Linq.Expressions;

namespace Triade.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // One interface is an abstract entity to reflect the real class functions
        Task<T> Get(int id);

        Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null);
        Task<T> GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null);

        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Remove(int id);
        Task<bool> Exists(int id);
        Task<bool> Save();
    }
}