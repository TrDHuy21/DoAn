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
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public IFormFile formFile { get; set; }
        public ImageFileDto Image { get; set; }
        public string Description { get; set; }
    }
}
