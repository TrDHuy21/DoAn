using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Application.Dtos.OrderDtos
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }
        public string? EmployeeNote { get; set; }
        public string? CustomerNote { get; set; }
        public int Status { get; set; }
        public string? TrackingCode { get; set; }
    }
}
