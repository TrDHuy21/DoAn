using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ProductAttributeDtos
{
    public class IndexProductAttributeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDisplay { get; set; }
        public bool CanFilter { get; set; }
        public bool IsActive { get; set; }
    }
}
