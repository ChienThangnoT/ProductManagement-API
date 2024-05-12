using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SE161774.ProductManagement.Repo.Interface
{
    public interface IGenericRepository<T> where T : class
    {
       Task<T> GetByIdAsync(int id);
       // T GetByUserName(string name);
       // IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null, // Optional parameter for pagination (page number)
            int? pageSize = null);
      //  IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        void Update(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
