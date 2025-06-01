using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class ProductAttribute : BaseEntity
    {
        public bool IsDisplay { get; set; }
        public bool CanFilter { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public IEnumerable<ProductDetailAttribute>? ProductDetailAttributes { get; set; }
    }
}
