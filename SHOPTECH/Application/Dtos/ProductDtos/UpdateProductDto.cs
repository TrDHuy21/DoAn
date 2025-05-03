using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Application.Dtos.ImageDtos;

namespace Application.Dtos.ProductDtos
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự")]
        public string Name { get; set; }

        [StringLength(2000, ErrorMessage = "Mô tả không được vượt quá 2000 ký tự")]
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public IFormFile? FormFile { get; set; }

        public ImageFileDto? MainImage { get; set; }

        public int BrandId { get; set; }

        public int CategoryId { get; set; }

    }
}
