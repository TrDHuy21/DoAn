using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class Order 
    {
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? status { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? TrackingCode { get; set; }
        public string? Note { get; set; }
        
        //employee
        public int EmployeeId { get; set; }
        public User? Employee { get; set; }

        //customer  
        public int CustomerId { get; set; }
        public User? Customer { get; set; }

        // ward
        public int WardId { get; set; }
        public Ward? Ward { get; set; }

        public IEnumerable<OrderDetail>? OrderDetails { get; set; }

    }
}
