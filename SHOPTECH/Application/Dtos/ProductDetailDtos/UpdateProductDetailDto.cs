using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Application.Dtos.ProductDetailAttributeDtos;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.ProductDetailDtos
{
    public class UpdateProductDetailDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
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

        public ImageFileDto? Image { get; set; }

        public IFormFile? FormFile { get; set; }
        public List<ProductDetailAttributeDto>? ProductDetailAttributeDtos { get; set; }


    }
}
