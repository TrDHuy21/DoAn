using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Application.Dtos.ProductDetailAttributeDtos;

namespace Application.Dtos.ProductDetailDtos
{
    public class DetailProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsHot { get; set; }
        public bool? IsSale { get; set; }
        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductBrandId { get; set; }
        public int? ImageId { get; set; }
        public ImageFileDto? Image { get; set; }
        public List<ProductDetailAttributeDto>? ProductDetailAttributes { get; set; }
    }
}
