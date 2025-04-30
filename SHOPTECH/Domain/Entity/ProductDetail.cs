using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Enity
{
    public class ProductDetail : BaseEntity
    {
		public string? Code { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
		public bool? isNew { get; set; }
        public bool? isHot { get; set; }
        public bool? isSale { get; set; }
        public string? Color { get; set; }
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }

        //image
        public int? ImageId { get; set; }
        public virtual ImageFile? Image { get; set; }

        public virtual IEnumerable<ProductDetailAttribute>? ProductDetailAttributes { get; set; }
        public virtual IEnumerable<Cart>? Carts { get; set; }
        public virtual IEnumerable<OrderDetail>? OrderDetails { get; set; }

        public virtual IEnumerable<ProductDetailImage>? ProductDetailImages { get; set; }

    }
}
