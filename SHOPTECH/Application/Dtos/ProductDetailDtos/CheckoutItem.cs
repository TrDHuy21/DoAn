using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;

namespace Application.Dtos.ProductDetailDtos
{
    public class CheckoutItem
    {
        public int productDetailId { get; set; }
        public IndexProductDetailDto ProductDetail { get; set; }
        public int Quantity { get; set; }
    }
}
