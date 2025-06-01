using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.OrderDetailDtos;

namespace Application.Dtos.OrderDtos
{
    public class CreateOfflineOrderDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string WardId { get; set; }
        public IEnumerable<CreateOrderDetailDto> OrderDetails { get; set; }
    }
}
