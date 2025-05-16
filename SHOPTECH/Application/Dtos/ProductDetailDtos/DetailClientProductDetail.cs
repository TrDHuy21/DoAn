using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.CategoryDtos;
using Application.Dtos.ImageDtos;
using Application.Dtos.ProductDetailAttributeDtos;
using Application.Dtos.ProductDtos;

namespace Application.Dtos.ProductDetailDtos
{
    public class DetailClientProductDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsHot { get; set; }
        public bool? IsSale { get; set; }
        public string? ColorName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryUrlName { get; set; }
        public List<ProductDetailAttributeDto>? ProductDetailAttributes { get; set; }
    }
}
