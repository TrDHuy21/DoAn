using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.ProductImage
{
    public class ProductImageDto
    {
        public int ImageFileId { get; set; }
        public int ProductId { get; set; }
        public IFormFile? FormFile { get; set; }

        public ImageFileDto? Image { get; set; }
        public string? Description { get; set; }
    }
}
