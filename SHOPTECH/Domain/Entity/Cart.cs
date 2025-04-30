using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class Cart
    {
        public int Quantity { get; set; }

        //productdetail
        public int ProductDetailId { get; set; }
        public ProductDetail? ProductDetail { get; set; }

        //customer
        public int UserId { get; set; }
        public User? User { get; set; }

    }
}
