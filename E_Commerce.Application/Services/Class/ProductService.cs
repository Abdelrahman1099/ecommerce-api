using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Class
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductSevcie
    {
        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(ct);
            var brandDtos = mapper.Map<IReadOnlyList<BrandDto>>(brands);
            return Result<IReadOnlyList<BrandDto>>.Ok(brandDtos);
        }

        public async Task<Result<PaginationResult<ProductDto>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default)
        {
            var specs = new ProductWithBrandAndTypeSpecifications(queryParams);


            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(specs, ct);
            var productDtos = mapper.Map<IReadOnlyList<ProductDto>>(products);
            var countSpecs = new ProductsCountSpecifications(queryParams);
            var count = await unitOfWork.GetRepository<Product, int>().CountAsync(countSpecs, ct);
            var value = new PaginationResult<ProductDto>(queryParams.PageIndex, queryParams.PageSize, count, productDtos);
            return Result<PaginationResult<ProductDto>>.Ok(value);
        }

        public async Task<Result<IReadOnlyList<TypeDto>>> GetAllTypeAsync(CancellationToken ct = default)
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var typeDtos = mapper.Map<IReadOnlyList<TypeDto>>(types);
            return Result<IReadOnlyList<TypeDto>>.Ok(typeDtos);
        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken ct = default)
        {

            var specs = new ProductWithBrandAndTypeSpecifications(id);

            var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(specs, ct);
            if(product is null)
            {
                return Result<ProductDto>.Fail(Error.NotFound("Product.NotFound", $"Product With Id {id} Is Not Found !"));
            }

            var productDto = mapper.Map<ProductDto>(product);

            return Result<ProductDto>.Ok(productDto);
        }
    }
}
