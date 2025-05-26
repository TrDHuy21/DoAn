using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.OrderDetailDtos
{
    public class CreateOrderDetailDto
    {
        public int Quantity { get; set; }
        public int ProductDetailId { get; set; }
    }
}
