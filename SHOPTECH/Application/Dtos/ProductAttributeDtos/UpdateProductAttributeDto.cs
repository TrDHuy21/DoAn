using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ProductAttributeDtos
{
    public class UpdateProductAttributeDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [DefaultValue(false)]
        public bool IsDisplay { get; set; } = false;

        [DefaultValue(false)]
        public bool IsActive { get; set; } = true;

        [DefaultValue(false)]
        public bool? CanFilter { get; set; } = false;
    }
}
