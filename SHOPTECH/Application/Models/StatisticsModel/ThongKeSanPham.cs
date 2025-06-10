using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.StatisticsModel
{
    public class ThongKeSanPham
    {
        public List<TopSanPham> TopSanPhamTheoSoLuong { get; set; }
        public List<TopSanPham> TopSanPhamTheoDoanhThu { get; set; }
    }
}
