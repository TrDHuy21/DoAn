using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Application.Dtos.ProductDtos;
using AutoMapper;
using Domain.Enity;

namespace Application.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, IndexProductDto>()
                .ForMember(t => t.BrandName, o => o.MapFrom(s => s.Brand != null ? s.Brand.Name : null))
                .ForMember(t => t.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : null))
                .ForMember(t => t.MainImage, x => x.MapFrom(s => s.MainImage != null ? new ImageFileDto
                {
                    Data = s.MainImage.Data,
                    Type = s.MainImage.Type,
                } : null));

            CreateMap<Product, DetailProductDto>()
               .ForMember(t => t.MainImage, x => x.MapFrom(s => s.MainImage != null ? new ImageFileDto
               {
                   Data = s.MainImage.Data,
                   Type = s.MainImage.Type,
               } : null));

            CreateMap<AddProductDto, Product>();

            CreateMap<Product, UpdateProductDto>()
                .ForMember(t => t.MainImage, x => x.MapFrom(s => s.MainImage != null ? new ImageFileDto
                {
                    Id = s.MainImage.Id,
                    Name = s.MainImage.Name,
                    Data = s.MainImage.Data,
                    Type = s.MainImage.Type
                } : null)); 

            CreateMap<UpdateProductDto, Product>()
                .ForMember(t => t.MainImage, o => o.Ignore())
                .ForMember(t => t.MainImageId, o => o.Ignore());







        }
    }
}
