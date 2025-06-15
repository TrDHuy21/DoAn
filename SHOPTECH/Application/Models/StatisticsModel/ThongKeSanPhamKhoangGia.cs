using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.StatisticsModel
{
    public class ThongKeSanPhamKhoangGia
    {
        //KhoangGia = range.Label,
        //            GiaTrungBinh = Math.Round(averagePrice),
        //            TongSoLoaiSanPham = totalProducts,
        //            TongSoSanPhamBanDuoc = totalSold

        public string KhoangGia { get; set; }
        public decimal GiaTrungBinh { get; set; }
        public int TongSoLoaiSanPham { get; set; }
        public int TongSoSanPhamBanDuoc { get; set; }
    }
}
