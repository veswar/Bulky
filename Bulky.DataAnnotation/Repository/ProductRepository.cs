using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        internal readonly ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext dbContext):base(dbContext)
        {
                _dbContext = dbContext;
        }
        public void update(Product product)
        {
            _dbContext.Update(product);
        }
    }
}
