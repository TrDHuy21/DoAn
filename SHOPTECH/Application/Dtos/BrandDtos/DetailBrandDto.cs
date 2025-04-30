using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Domain.Enity;

namespace Application.Dtos.BrandDtos
{
    public class DetailBrandDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        //public int? CreatedBy { get; set; }

        //public int? UpdatedBy { get; set; }

        //public string CreatedByUser { get; set; }
        //public string UpdatedByUser { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public int? ImageId { get; set; }
        public ImageFileDto? Image { get; set; }

    }
}
