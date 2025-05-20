using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.OrderDetailDtos;

namespace Application.Dtos.OrderDtos
{
    public class CreateOrderOnlineDto
    {
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        //customer  
        public int CustomerId { get; set; }
        // ward
        public int WardId { get; set; }
        public IEnumerable<OrderDetailDto>? OrderDetails { get; set; }
    }
}
