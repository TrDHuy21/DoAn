using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;

namespace Application.Dtos.ProductDtos
{
    public class IndexProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public ImageFileDto? MainImage { get; set; }
    }
}
