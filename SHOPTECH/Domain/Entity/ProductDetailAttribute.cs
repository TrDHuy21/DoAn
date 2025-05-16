using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class ProductDetailAttribute
    {
        public string? Value { get; set; }
        public string? UrlValue { get; set; }

        public int? ImageId { get; set; }
        public ImageFile? Image { get; set; }

        public int ProductDetailId { get; set; }
        public ProductDetail? ProductDetail { get; set; }

        public int ProductAttributeId { get; set; }
        public ProductAttribute? ProductAttribute { get; set; }



    }
}
