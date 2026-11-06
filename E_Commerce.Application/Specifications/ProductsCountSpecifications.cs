using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class ProductsCountSpecifications : BaseSpecificatopns<Product, int>
    {
        public ProductsCountSpecifications(ProductQueryParams queryParams) : base
           (
               p =>
               (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId)
               &&
               (!queryParams.TypeID.HasValue || p.TypeId == queryParams.TypeID)
               &&
               (string.IsNullOrWhiteSpace(queryParams.searchValue) || p.Name.ToLower().Contains(queryParams.searchValue.ToLower()))
           )
        {
        }
    }
}