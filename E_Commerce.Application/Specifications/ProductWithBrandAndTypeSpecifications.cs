using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecificatopns<Product, int>
    {
        public ProductWithBrandAndTypeSpecifications(ProductQueryParams queryParams) : base
            (
                p =>
                (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId)
                &&
                (!queryParams.TypeID.HasValue || p.TypeId == queryParams.TypeID)
                &&
                (string.IsNullOrWhiteSpace(queryParams.searchValue) || p.Name.ToLower().Contains(queryParams.searchValue.ToLower()))
            )
        {
            AddInclude(p => p.Type);
            AddInclude(p => p.Brand);
               
            switch(queryParams.Sort)
            {
                    case ProductSortOptions.NameAsc:    
                    AddOrderBy(p => p.Name);
                        break;
                    case ProductSortOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                        break;
                    case ProductSortOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                        break;
                    case ProductSortOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                        break;

                    default:
                    AddOrderBy(p => p.Id);
                        break;  
            }

            ApplyPagination(queryParams.PageIndex, queryParams.PageSize);
        }
        public ProductWithBrandAndTypeSpecifications(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.Type);
            AddInclude(p => p.Brand);
               
        }
    }
}
