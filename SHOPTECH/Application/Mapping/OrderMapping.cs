using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.AddressDtos;
using Application.Dtos.OrderDetailDtos;
using Application.Dtos.OrderDtos;
using Application.Dtos.ProductDetailDtos;
using Application.Dtos.RoleDtos;
using Application.Dtos.UserDtos;
using AutoMapper;
using Domain.Enity;

namespace Application.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping() {
            CreateMap<CreateOnlineOrderDto, Order>()
                .ForMember(t => t.OrderDetails, o => o.Ignore())
                .ForMember(t => t.Status, o => o.MapFrom(t=> 0))
                .ForMember(t => t.OrderDate, o => o.MapFrom(t=> DateTime.Now))
                .ForMember(t => t.Type, o => o.MapFrom(t=> "online"));


            CreateMap<CreateOfflineOrderDto, Order>()
                .ForMember(t => t.OrderDetails, o => o.Ignore())
                .ForMember(t => t.Status, o => o.MapFrom(t => 5))
                .ForMember(t => t.OrderDate, o => o.MapFrom(t => DateTime.Now))
                .ForMember(t => t.Type, o => o.MapFrom(t => "offline"));

            CreateMap<Order, IndexOrderDto>()
                .ForMember(t => t.TotalAmount, o => o.MapFrom(s => s.OrderDetails.Sum(x => x.Quantity * x.Price)))
                .ForMember(t => t.Customer, o => o.MapFrom(s => new IndexUserDto()
                {
                    Id = s.Customer.Id,
                    Name = s.Customer.Name,
                    Phone = s.Customer.Phone,
                    Email = s.Customer.Email,
                }))
                .ForMember(t => t.Employee, o => o.MapFrom(s => new IndexUserDto()
                {
                    Id = s.Employee.Id,
                    Name = s.Employee.Name,
                    Phone = s.Employee.Phone,
                    Email = s.Employee.Email,
                }));



            CreateMap<Order, DetailOrderDto>()
                //adress
                .ForMember(t => t.Ward, o => o.MapFrom(s => new WardDto()
                {
                    Id = s.WardId,
                    Name = s.Ward.Name
                }))
                .ForMember(t => t.District, o => o.MapFrom(s => new DistrictDto()
                {
                    Id = s.Ward.District.Id,
                    Name = s.Ward.District.Name
                }))
                .ForMember(t => t.Province, o => o.MapFrom(s => new ProvinceDto()
                {
                    Id = s.Ward.District.Province.Id,
                    Name = s.Ward.District.Province.Name
                }))
                //user: customer and employee
                .ForMember(t => t.Customer, o => o.MapFrom(s => new IndexUserDto()
                {
                    Id = s.Customer.Id,
                    Name = s.Customer.Name,
                    Phone = s.Customer.Phone,
                    Email = s.Customer.Email,
                }))
                .ForMember(t => t.Employee, o => o.MapFrom(s => new IndexUserDto()
                {
                    Id = s.Employee.Id,
                    Name = s.Employee.Name,
                    Phone = s.Employee.Phone,
                    Email = s.Employee.Email,
                }))
                .ForMember(t => t.OrderDetails, o => o.Ignore());
                //orderdetail list (su ly trong controller
        }
    }
}
