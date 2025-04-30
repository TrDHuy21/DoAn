using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class OrderDetail
    {

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? sale; 

        //order
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        //product
        public int ProductDetailId { get; set; }
        public ProductDetail? ProductDetail { get; set; }

        

    }
}
