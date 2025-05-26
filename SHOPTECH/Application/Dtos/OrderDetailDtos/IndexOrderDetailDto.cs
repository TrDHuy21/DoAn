using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ProductDetailDtos;

namespace Application.Dtos.OrderDetailDtos
{
    public class IndexOrderDetailDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; } 
        public IndexProductDetailDto ProductDetail { get; set; }
    }
}
