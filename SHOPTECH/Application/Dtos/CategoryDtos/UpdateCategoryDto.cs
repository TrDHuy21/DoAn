using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.CategoryDtos
{
    public  class UpdateCategoryDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        // Cho phép người dùng cập nhật hình ảnh (nếu có)
        public IFormFile? FormFile { get; set; }
        public ImageFileDto? Image { get; set; }
    }
}
