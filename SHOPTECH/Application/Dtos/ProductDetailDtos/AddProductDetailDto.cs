using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ProductDetailAttributeDtos;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.ProductDetailDtos
{
    public class AddProductDetailDto
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        public string? Code { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        [DefaultValue(false)]
        public bool? IsNew { get; set; }

        [DefaultValue(false)]
        public bool? IsHot { get; set; }

        [DefaultValue(false)]
        public bool? IsSale { get; set; }

        public string? ColorName { get; set; }

        public string? ColorCode { get; set; }

        [Required]
        public int? ProductId { get; set; }

        public IFormFile? FormFile { get; set; }
        public List<ProductDetailAttributeDto>? ProductDetailAttributes { get; set; }
    }
}
