using E_Commerce.API.Attributes;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.API.Controllers
{
    public class ProductsController(IProductSevcie productServcie) : ApiBaseController
    {
        [HttpGet]
        [RedisCach(60)]
        [Authorize]
        public async Task<ActionResult<PaginationResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams queryParams, CancellationToken ct = default) 
        {
            var result = await productServcie.GetAllProductsAsync(queryParams, ct);
            return ToActionResult(result);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken ct = default) 
        {
            var result = await productServcie.GetAllBrandsAsync(ct);
            return ToActionResult(result);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken ct = default) 
        {
            var result = await productServcie.GetAllTypeAsync(ct);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id, CancellationToken ct = default)
        {
            var result = await productServcie.GetProductByIdAsync(id, ct);
            return ToActionResult(result);
        }

    }
}
