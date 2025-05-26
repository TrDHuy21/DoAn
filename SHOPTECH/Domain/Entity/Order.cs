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

        /*
        0. chờ xác nhận
        1. đã xác nhận
        2. đang giao hàng
        3. giao hàng thành công
        4. đơn hàng thành công
        5. giao hàng không thành công
        6. đang hoàn hàng
        7. hoàn hàng thành công
        8. đã Hủy
         */
        public int? Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? TrackingCode { get; set; }
        public string? Type { get; set; }

        //customer  
        public int? CustomerId { get; set; }
        public User? Customer { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? CustomerNote { get; set; }


        //address
        public string? Address { get; set; }
        public string? WardId { get; set; }
        public Ward? Ward { get; set; }

        //employee
        public int? EmployeeId { get; set; }
        public User? Employee { get; set; }
        public string? EmployeeNote { get; set; }

        public IEnumerable<OrderDetail>? OrderDetails { get; set; }
    }
}
