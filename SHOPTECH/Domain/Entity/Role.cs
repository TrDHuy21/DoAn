using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public IEnumerable<User>? Employees { get; set; }
    }
}
