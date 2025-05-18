using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.CategoryDtos;
using Application.Dtos.ImageDtos;
using Application.Dtos.ProductDetailAttributeDtos;
using Application.Dtos.ProductDetailDtos;
using Application.Dtos.ProductDtos;
using Application.Helper;
using AutoMapper;
using Domain.Enity;

namespace Application.Mapping
{
    public class ProductDetailMapping : Profile
    {
        public ProductDetailMapping()
        {
            CreateMap<ProductDetail, IndexProductDetailDto>()
                .ForMember(t => t.ProductDetailAttributes, o=> o.MapFrom(s => s.ProductDetailAttributes.Select(
                    pda => new ProductDetailAttributeDto
                    {
                        Value = pda.Value,
                        ProductDetailId = pda.ProductDetailId,
                        ProductAttributeId = pda.ProductAttributeId,
                        ProductAttributeName = pda.ProductAttribute.Name
                    }
                    )));


            CreateMap<ProductDetail, DetailProductDetailDto>()
                .ForMember(t => t.Image, x => x.MapFrom(s => s.Image != null ? new ImageFileDto
                {
                    Id = s.Image.Id,
                    Name = s.Image.Name,
                    Data = s.Image.Data,
                    Type = s.Image.Type
                } : null))
                .ForMember(t => t.ProductDetailAttributes, o => o.MapFrom(s => s.ProductDetailAttributes.Select(
                    pda => new ProductDetailAttributeDto
                    {
                        Value = pda.Value,
                        ProductDetailId = pda.ProductDetailId,
                        ProductAttributeId = pda.ProductAttributeId,
                        ProductAttributeName = pda.ProductAttribute.Name
                    }
                    )));

            CreateMap<UpdateProductDetailDto, ProductDetail>()
                .ForMember(dest => dest.ProductDetailAttributes, opt => opt.Ignore())
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.ImageId, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(t => t.UrlName, o => o.MapFrom(s => s.Name.ChuanHoa("_")));

            CreateMap<AddProductDetailDto, ProductDetail>()
                .ForMember(dest => dest.ProductDetailAttributes, opt => opt.Ignore())
                .ForMember(t => t.UrlName, o => o.MapFrom(s => s.Name.ChuanHoa("_")));

            CreateMap<ProductDetail, DetailClientProductDetail>()
                .ForMember(t => t.ProductDetailAttributes, o => o.MapFrom(s => s.ProductDetailAttributes.Select(
                    pda => new ProductDetailAttributeDto
                    {
                        Value = pda.Value,
                        ProductDetailId = pda.ProductDetailId,
                        ProductAttributeId = pda.ProductAttributeId,
                        ProductAttributeName = pda.ProductAttribute.Name
                    }
                    )))
                .ForMember(t => t.ProductId, o => o.MapFrom(s => s.Product.Id == null ? 0 : s.Product.Id))
                .ForMember(t => t.ProductName, o => o.MapFrom(s => s.Product.Name == null ? "" : s.Product.Name))
                .ForMember(t => t.CategoryId, o => o.MapFrom(s => s.Product.CategoryId))
                .ForMember(t => t.CategoryName, o => o.MapFrom(s => s.Product.Category.Name == null ? "" : s.Product.Category.Name))
                .ForMember(t => t.CategoryUrlName, o => o.MapFrom(s => s.Product.Category.UrlName == null ? "" : s.Product.Category.UrlName));


        }
    }
}
