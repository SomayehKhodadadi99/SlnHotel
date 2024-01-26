using Hotel.Data;
using Hotel.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;
using X.PagedList;

namespace Hotel.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataBaseContext _context;

        private DbSet<T> _dbSet;

        public GenericRepository(DataBaseContext context)
        {
                _context = context;
                _dbSet = _context.Set<T>();
            
        }
        public async Task Delete(int Id)
        {
          var entity = await _dbSet.FindAsync(Id);
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        //public  async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        //{

        //    IQueryable<T> query = _dbSet;
        //    if (includes != null)
        //    {
        //        foreach (var item in includes)

        //            query = query.Include(item);

        //    }
        //    return await query.AsNoTracking<T>().FirstOrDefaultAsync(expression);
        //}


        public async Task<T> Get(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
         

            IQueryable<T> query = _dbSet;

            if (includes != null)
                foreach (var item in includes)
                    query = query.Include(item);

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }



        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (expression!=null)
            {
                query = query.Where(expression);
            }

            if (orderBy != null)
            {
                query=orderBy(query);
            }

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IPagedList<T>> GetPageedList(QueryParameters queryParameters,List<string> includes = null)
        {
            IQueryable<T> query = _dbSet;

           

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            return await query.AsNoTracking().ToPagedListAsync(queryParameters.PageNumber,queryParameters.PageSize);
        }

        public async Task Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task  InsertRange(IEnumerable<T> entities)
        {
             await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

       
    }
}
