using AutoMapper;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.MappingProfiles.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypeDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(D => D.ProductBrand, opt => opt.MapFrom(s => s.Brand.Name))
                .ForMember(D => D.ProductType, opt => opt.MapFrom(s => s.Type.Name))
                .ForMember(D => D.PictureUrl, opt => opt.MapFrom(s => $"{s.PictureUrl}"))
                .ForMember(D => D.PictureUrl, opt => opt.MapFrom<PictureUrlResolver>());
        }
    }
}
