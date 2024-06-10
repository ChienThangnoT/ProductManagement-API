using Microsoft.EntityFrameworkCore;
using SE161774.ProductManagement.Repo.Helpers;
using SE161774.ProductManagement.Repo.Repository;
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
            int? pageIndex = 1, // Optional parameter for pagination (page number)
            int? pageSize = 20);

        Task<Pagination<T>> ToPaginationAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = 1, // Optional parameter for pagination (page number)
            int? pageSize = 20);
      //  IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        void Update(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
