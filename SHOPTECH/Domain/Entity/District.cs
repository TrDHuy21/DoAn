 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class District
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int? ProvinceId { get; set; }
        public Province? Province { get; set; }

        public IEnumerable<Ward>? Wards { get; set; }
    }
}
 