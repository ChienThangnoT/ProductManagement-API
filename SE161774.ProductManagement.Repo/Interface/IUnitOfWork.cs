using SE161774.ProductManagement.Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE161774.ProductManagement.Repo.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Category> CategorysRepository { get; }
        IGenericRepository<Member> MembersRepository { get; }
        IGenericRepository<Order> OrdersRepository { get; }
        IGenericRepository<OrderDetail> OrderDetailsRepository { get; }
        IGenericRepository<Product> ProductsRepository { get; }
        int Save();
    }
}
