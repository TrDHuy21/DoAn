using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.StatisticsModel
{
    public class TopDanhMuc
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public int SoLuong { get; set; }
        public decimal DoanhThu { get; set; }
    }
}
