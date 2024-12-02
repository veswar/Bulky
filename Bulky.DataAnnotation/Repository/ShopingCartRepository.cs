using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ShopingCartRepository : Repository<ShopingCart>, IShopingCartRepository
    {
        readonly ApplicationDbContext _context;
        public ShopingCartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        void IShopingCartRepository.Update(ShopingCart shopingCart)
        {
            _context.Update(shopingCart);
        }
    }
}
