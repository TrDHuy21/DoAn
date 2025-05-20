using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enity;

namespace Application.Dtos.OrderDetailDtos
{
    public class OrderDetailDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? sale;
        //order
        public int OrderId { get; set; }

        //product
        public int ProductDetailId { get; set; }
    }
}
