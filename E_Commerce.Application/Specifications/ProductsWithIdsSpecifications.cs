using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class ProductsWithIdsSpecifications : BaseSpecificatopns<Product, int>
    {
        public ProductsWithIdsSpecifications(HashSet<int> ids) : base(p => ids.Contains(p.Id))
        {
            
        }
    }
}
