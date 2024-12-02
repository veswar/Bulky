using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository category { get; }
        IProductRepository product { get; }
        IShopingCartRepository shopingCart { get; }
        IApplicationUserRepository applicationUser { get; }
        IOrderDetailsRepository orderDetails { get; }
        IOrderHeaderRepository orderHeader { get; }
        void Save();
    }
}
