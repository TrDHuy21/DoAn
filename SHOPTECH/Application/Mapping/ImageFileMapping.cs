using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using AutoMapper;
using Domain.Enity;

namespace Application.Mapping
{
    public class ImageFileMapping : Profile
    {
        public ImageFileMapping()
        {
            CreateMap<ImageFile, ImageFileDto>().ReverseMap();
        }
    }
}
