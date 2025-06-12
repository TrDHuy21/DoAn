using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.StatisticsModel
{
    public class ThongKeNhanVien
    {
        public string TenNhanVien { get; set; }
        public List<decimal> DoanhSo { get; set; }
        public List<int> DonHang { get; set; }

    }
}
