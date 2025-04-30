using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ProductAttributeDtos
{
    public class AddProductAttributeDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [DefaultValue(false)]
        public bool IsDisplay { get; set; }

        [DefaultValue(false)]
        public bool IsActive { get; set; }

        [DefaultValue(false)]
        public bool? CanFilter { get; set; }

        [Required]
        public int CategoryId { get; set; }

      
    }
}
