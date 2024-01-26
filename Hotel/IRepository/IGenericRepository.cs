using System.Linq.Expressions;
using X.PagedList;

namespace Hotel.IRepository
{
    public interface IGenericRepository<T> where T : class
    {

        Task Insert(T entity);

        void Update(T entity);

         Task Delete(int Id);

        void DeleteRange(IEnumerable<T> entities);

        Task InsertRange(IEnumerable<T> entities);

        Task<IList<T>> GetAll(Expression<Func<T,bool>> expression=null
            ,Func<IQueryable<T>,IOrderedQueryable<T>> orderBy=null
            ,List<string> includes=null);


        Task<T> Get(Expression<Func<T,bool>> expression=null,List<string> includes=null);


        Task<IPagedList<T>> GetPageedList(QueryParameters queryParameters,List<string> includes = null);

    }
}
