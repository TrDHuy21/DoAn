using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enity;

namespace Application.Dtos.ProductDetailAttributeDtos
{
    public class ProductDetailAttributeDto
    {
        [DefaultValue("")]
        public string Value { get; set; }
        public int ProductDetailId { get; set; }
        public string? ProductDetailName { get; set; }

        public int ProductAttributeId { get; set; }
        public string? ProductAttributeName { get; set; }
    }
}
