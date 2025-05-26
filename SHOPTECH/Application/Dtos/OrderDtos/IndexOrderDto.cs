using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.UserDtos;

namespace Application.Dtos.OrderDtos
{
    public class IndexOrderDto
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public string TrackingCode { get; set; }
        public string Type { get; set; }
        public decimal TotalAmount { get; set; }
        public IndexUserDto Customer { get; set; }
        public IndexUserDto Employee { get; set; }
    }
}
