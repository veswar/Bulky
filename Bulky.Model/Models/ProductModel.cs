using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Model.Models
{
    public class ProductModel
    {
        public Product Product { get; set; }
        [ValidateNever]
        public List<SelectListItem> Categories { get; set; }
    }
}
