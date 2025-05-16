using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.BrandDtos;
using Application.Dtos.ImageDtos;
using Application.Helper;
using AutoMapper;
using Domain.Enity;

namespace Application.Mapping
{
    public class BrandMapping : Profile
    {
        public BrandMapping()
        {
            CreateMap<AddBrandDto, Brand>()
                .ForMember(t => t.UrlName, o => o.MapFrom(s => s.Name.ChuanHoa("_")));


            CreateMap<Brand, IndexBrandDto>()
                .ForMember(t => t.Image, x => x.MapFrom(s => s.Image != null ? new ImageFileDto
                {
                    Data = s.Image.Data,
                    Type = s.Image.Type,
                } : null));

            CreateMap<Brand, DetailBrandDto>()
                //.ForMember(t => t.CreatedBy, x => x.MapFrom(s => s.CreatedByUser != null ? s.CreatedByUser.Username : null))
                //.ForMember(t => t.UpdatedBy, x => x.MapFrom(s => s.UpdatedByUser != null ? s.UpdatedByUser.Username : null))
                .ForMember(t => t.Image, x => x.MapFrom(s => s.Image != null ? new ImageFileDto
                {
                    Id = s.Image.Id,
                    Name = s.Image.Name,
                    Data = s.Image.Data,
                    Type = s.Image.Type
                } : null));

            CreateMap<Brand, UpdateBrandDto>()
                .ForMember(t => t.Image, x => x.MapFrom(s => s.Image != null ? new ImageFileDto
                {
                    Id = s.Image.Id,
                    Name = s.Image.Name,
                    Data = s.Image.Data,
                    Type = s.Image.Type
                } : null));

            CreateMap<UpdateBrandDto, Brand>()
                .ForMember(t => t.Image, o => o.Ignore())
                .ForMember(t => t.ImageId, o => o.Ignore())
                .ForMember(t => t.UrlName, o => o.MapFrom(s => s.Name.ChuanHoa("_")));

        }
    }
}
