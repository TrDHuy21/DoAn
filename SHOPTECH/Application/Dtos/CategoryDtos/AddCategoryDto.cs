using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Application.Dtos.ProductAttributeDtos;

namespace Application.Dtos.CategoryDtos
{
    public class AddCategoryDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile FormFile { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
