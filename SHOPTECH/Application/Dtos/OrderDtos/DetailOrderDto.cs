using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.AddressDtos;
using Application.Dtos.OrderDetailDtos;
using Application.Dtos.UserDtos;
using Domain.Enity;

namespace Application.Dtos.OrderDtos
{
    public class DetailOrderDto
    {

        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? TrackingCode { get; set; }
        public string? Type { get; set; }

        //customer  
        public IndexUserDto? Customer { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? CustomerNote { get; set; }


        //address
        public string Address { get; set; }
        public string WardId { get; set; }
        public WardDto? Ward { get; set; }
        public DistrictDto? District { get; set; }
        public Province? Province { get; set; }

        //employee
        public IndexUserDto? Employee { get; set; }
        public string? EmployeeNote { get; set; }

        public ICollection<IndexOrderDetailDto> OrderDetails { get; set; }
    }
}
