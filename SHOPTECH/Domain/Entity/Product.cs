using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Enity
{
    public class Product : BaseEntity
    {
        public int? MainImageId { get; set; }
        public ImageFile? MainImage { get; set; }

        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public IEnumerable<ProductDetail>? ProductDetails { get; set; }
        public virtual IEnumerable<ProductImage>? ProductImages { get; set; }

    }
}
