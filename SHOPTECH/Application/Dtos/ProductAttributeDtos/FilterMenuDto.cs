using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ProductAttributeDtos
{
    public class FilterMenuDto
    {
        public string Name { get; set; }
        public string UrlName { get; set; }
        public List<string?> Values { get; set; }
        public List<string?> UrlValues { get; set; }

    }
}
