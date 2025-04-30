using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ProductAttributeDtos;
using AutoMapper;
using Domain.Enity;

namespace Application.Mapping
{
    public class ProductAttributeMapping : Profile
    {
        public ProductAttributeMapping() {
            CreateMap<AddProductAttributeDto, ProductAttribute>();

            CreateMap<UpdateProductAttributeDto, ProductAttribute>()
                .ForMember(t => t.CategoryId, o => o.Ignore());

            CreateMap<ProductAttribute, UpdateProductAttributeDto>();

            CreateMap<ProductAttribute, IndexProductAttributeDto>();

        }
    }
}
