using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.AddressDtos;
using AutoMapper;
using Domain.Enity;

namespace Application.Mapping
{
    internal class AddressMapping : Profile
    {
        public AddressMapping() { 
            CreateMap<ProvinceDto, Province>().ReverseMap();
            CreateMap<WardDto, Ward>().ReverseMap();
            CreateMap<DistrictDto, District>().ReverseMap();
        }
    }
}
