using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.StatisticsModel
{
    public class DoanhThuVaDonHang
    {
        public string Thang {  get; set; }
        public decimal DoanhThuOnline { get; set; }
        public int SoLuongOnline { get; set; }
        public decimal DoanhThuOffline { get; set; }
        public int SoLuongOffline { get; set; }


    }
}
