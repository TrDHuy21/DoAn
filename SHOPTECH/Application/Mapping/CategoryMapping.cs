using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.BrandDtos;
using Application.Dtos.CategoryDtos;
using Application.Dtos.ImageDtos;
using Application.Dtos.ProductAttributeDtos;
using AutoMapper;
using Domain.Enity;

namespace Application.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping() {
            CreateMap<AddCategoryDto, Category>();

            CreateMap<Category, IndexCategoryDto>()
                .ForMember(t => t.Image, x => x.MapFrom(s => s.Image != null ? new ImageFileDto
                {
                    Data = s.Image.Data,
                    Type = s.Image.Type,
                } : null));


            CreateMap<Category, DetailCategoryDto>()
                //.ForMember(t => t.CreatedBy, x => x.MapFrom(s => s.CreatedByUser != null ? s.CreatedByUser.Username : null))
                //.ForMember(t => t.UpdatedBy, x => x.MapFrom(s => s.UpdatedByUser != null ? s.UpdatedByUser.Username : null))
                .ForMember(t => t.Image, x => x.MapFrom(s => s.Image != null ? new ImageFileDto
                {
                    Id = s.Image.Id,
                    Name = s.Image.Name,
                    Data = s.Image.Data,
                    Type = s.Image.Type
                } : null))
                .ForMember(t => t.ProductAttributes, o => o.MapFrom(s => s.ProductAttributes == null ? null :
                        s.ProductAttributes.Select(
                            pa => new IndexProductAttributeDto
                            {
                                Id = pa.Id,
                                Name = pa.Name,
                                Description = pa.Description,
                                IsDisplay = pa.IsDisplay,
                                IsActive = pa.IsActive
                            }
                        ).ToList()
                ));

            CreateMap<Category, UpdateCategoryDto>()
                .ForMember(t => t.Image, x => x.MapFrom(s => s.Image != null ? new ImageFileDto
                {
                    Id = s.Image.Id,
                    Name = s.Image.Name,
                    Data = s.Image.Data,
                    Type = s.Image.Type
                } : null));

            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(t => t.Image, o => o.Ignore())
                .ForMember(t => t.ImageId, o => o.Ignore())
                .ForMember(t => t.ProductAttributes, o => o.Ignore());
        }
    }
}
