using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enity;

namespace Domain.Entity
{
    public class ProductDetailImage
    {
        public string? Description { get; set; }

        public int ProductDetailId { get; set; }
        public ProductDetail? ProductDetail { get; set; }

        public int ImageFileId { get; set; }
        public ImageFile? ImageFile { get; set; }
    }
}
