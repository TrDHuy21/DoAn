using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ProductAttributeDtos;
using Application.Helper;
using AutoMapper;
using Domain.Enity;

namespace Application.Mapping
{
    public class ProductAttributeMapping : Profile
    {
        public ProductAttributeMapping() {
            CreateMap<AddProductAttributeDto, ProductAttribute>()
                .ForMember(t => t.UrlName, o => o.MapFrom(s => s.Name.GenerateFilterName("_")));

            CreateMap<UpdateProductAttributeDto, ProductAttribute>()
                .ForMember(t => t.CategoryId, o => o.Ignore())
                .ForMember(t => t.UrlName, o => o.MapFrom(s => s.Name.GenerateFilterName("_")));


            CreateMap<ProductAttribute, UpdateProductAttributeDto>();

            CreateMap<ProductAttribute, IndexProductAttributeDto>();
            CreateMap<ProductAttribute, DetailProductAttributeDto>().ReverseMap();

        }
    }
}
