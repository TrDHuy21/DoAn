using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Application.Dtos.ProductDtos;
using Application.Dtos.ProductImage;
using Application.Helper;
using AutoMapper;
using Domain.Enity;
using static System.Net.Mime.MediaTypeNames;

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
               } : null))
               .ForMember(t => t.productImages, x => x.MapFrom(s => s.ProductImages.Select(i => new ProductImageDto
               {
                   ImageFileId = i.ImageFileId,
                   ProductId = s.Id,
                   Image = new ImageFileDto
                   {
                       Id = i.ImageFile.Id,
                       Name = i.ImageFile.Name,
                       Data = i.ImageFile.Data,
                       Type = i.ImageFile.Type
                   }
               }).ToList()));

            CreateMap<AddProductDto, Product>()
                .ForMember(t => t.UrlName, o => o.MapFrom(s => s.Name.ChuanHoa("_")));

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
                .ForMember(t => t.MainImageId, o => o.Ignore())
                .ForMember(t => t.UrlName, o => o.MapFrom(s => s.Name.ChuanHoa("_")));
        }
    }
}
