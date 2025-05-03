using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Enity
{
    public class ImageFile
    {
        public int Id;
        public string? Name { get; set; }
        public byte[]? Data { get; set; }
        public string? Type { get; set; }


        public IEnumerable<Brand>? Brands { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<ProductDetailAttribute>? ProductDetailAttributes { get; set; }
        public IEnumerable<ProductDetail>? ProductDetails { get; set; }
        public IEnumerable<User>? Employees { get; set; }

        public virtual IEnumerable<ProductDetailImage>? ProductDetailImages { get; set; }
        public virtual IEnumerable<ProductImage>? ProductImages { get; set; }




    }
}
