using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class Ward
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int? DistrictId { get; set; }
        public District? District { get; set; }

        //employees
        public IEnumerable<User>? Users { get; set; }
        //customers
        public IEnumerable<Order>? Orders { get; set; }

    }
}
