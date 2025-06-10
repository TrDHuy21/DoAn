using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.StatisticsModel
{
    public class ThongKeKhoangGia
    {
        public Dictionary<string, double> TyLeTheoKhoangGia { get; set; }
        public List<Dictionary<string, int>> DuLieuTheoThang { get; set; }
    }
}
