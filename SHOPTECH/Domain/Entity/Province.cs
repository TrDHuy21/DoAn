using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class Province
    {
        public string Id { get; set; }
        public string? Name { get; set; }

        public IEnumerable<District>? Districts { get; set; } = new List<District>();
    }
}
