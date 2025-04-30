using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class ProductAttribute : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsDisplay { get; set; }
        public bool CanFilter { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public IEnumerable<ProductDetailAttribute>? ProductDetailAttributes { get; set; }
    }
}
