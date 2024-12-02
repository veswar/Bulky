using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Model.Models
{
    public class ShopingCartModel
    {
        public IEnumerable<ShopingCart> ShopingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
