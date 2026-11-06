using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class ProductQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeID { get; set; }    
        public string? searchValue { get; set; }
        public ProductSortOptions Sort {  get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize{ get; set; } = 5;
    }
    public enum ProductSortOptions
    {
        None = 0,
        NameAsc = 1,
        NameDesc = 2,
        PriceAsc = 3,
        PriceDesc = 4
    }
}
