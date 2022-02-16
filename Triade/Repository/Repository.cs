using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Triade.Data;
using Triade.Repository.IRepository;

namespace Triade.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Constructor
        protected readonly ApplicationDbContext _context;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbset = context.Set<T>();
        }
        #endregion

        #region Get Individual Object
        public async Task<T> Get(int id)
        {
            return await dbset.FindAsync(id);
        }

        // With Filter and you could to Incllude new properties
        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                Parallel.ForEach(includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), (includeProperty) =>
                {
                    query = query.Include(includeProperty);
                });
            }

            return await query.FirstOrDefaultAsync();
        }
        #endregion

        #region Get List of Objects with filters
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                Parallel.ForEach(includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), (includeProperty) =>
                {
                    query = query.Include(includeProperty);
                });
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return await query.ToListAsync();
        }
        #endregion

        #region Add, Update, Delete Object
        public async Task<bool> Add(T entity)
        {
            dbset.Add(entity);
            return await Save();
        }

        public async Task<bool> Update(T entity)
        {
            dbset.Update(entity);
            return await Save();
        }

        public async Task<bool> Remove(int id)
        {
            T entityToRemove = await dbset.FindAsync(id);
            dbset.Remove(entityToRemove);
            
            return await Save();            
        }
        #endregion

        #region Verify if Exist
        public async Task<bool> Exists(int id)
        {
            var value = await dbset.FindAsync(id);
             //>= 0 ? true : false
            if (value != null)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }
        #endregion

        #region Save
        public async Task<bool> Save()
        {
           return await _context.SaveChangesAsync() >= 0;
        }
        #endregion
    }
}
