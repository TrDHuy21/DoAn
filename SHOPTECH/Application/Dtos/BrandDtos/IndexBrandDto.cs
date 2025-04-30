using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;

namespace Application.Dtos.BrandDtos
{
    public class IndexBrandDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ImageFileDto? Image { get; set; }
        public bool IsActive { get; set; }
    }
}
