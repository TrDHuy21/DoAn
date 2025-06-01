using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.OrderDetailDtos;

namespace Application.Dtos.OrderDtos
{
    public class CreateOnlineOrderDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Ward is required.")]
        public string WardId { get; set; }
        public string? CustomerNote { get; set; }
        public IEnumerable<CreateOrderDetailDto> OrderDetails { get; set; }
    }
}
