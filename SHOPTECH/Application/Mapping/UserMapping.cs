using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.UserDtos;
using AutoMapper;
using Domain.Enity;

namespace Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping() {
            // maping from dto to entity and entity to dto
            CreateMap<User, IndexUserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ReverseMap();

            CreateMap<User, DetailUserDto>();

            //adduserdto to user
            CreateMap<AddUserDto, User>();

            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UpdateUserDto>();

            CreateMap<DetailUserDto, UpdateUserDto>();
        }
    }
}
