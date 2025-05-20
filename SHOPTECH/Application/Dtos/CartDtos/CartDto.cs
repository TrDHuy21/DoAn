using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;

namespace Application.Dtos.CartDtos
{
    public class CartDto
    {
        public int ProductDetailId { get; set; }
        public string? ProductName { get; set; }
        public ImageFileDto? Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
    }   
}
