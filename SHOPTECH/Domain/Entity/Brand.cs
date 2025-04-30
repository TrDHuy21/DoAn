using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class Brand : BaseEntity
    {
        public int? ImageId { get; set; }
        public ImageFile? Image { get; set; }

        public IEnumerable<Product>? Products { get; set; } 
    }
}
