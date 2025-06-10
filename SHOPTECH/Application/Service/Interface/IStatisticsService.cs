using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.StatisticsModel;

namespace Application.Service.Interface
{
    public interface IStatisticsService
    {
        // Tổng quan doanh thu và đơn hàng
        Task<List<DoanhThuVaDonHang>> DoanhThuVoiDonHang(int beginMonth, int beginYear, int endMonth, int endYear);
        Task<dynamic> SoLuongDonHang(int beginMonth, int beginYear, int endMonth, int endYear);

        // Thống kê khách hàng
        Task<ThongKeKhachHang> ThongKeKhachHang(int beginMonth, int beginYear, int endMonth, int endYear);

        // Thống kê nhân viên
        Task<List<ThongKeNhanVien>> ThongKeNhanVien(int beginMonth, int beginYear, int endMonth, int endYear);

        // Thống kê sản phẩm
        Task<ThongKeSanPham> ThongKeSanPham(int beginMonth, int beginYear, int endMonth, int endYear);

        // Thống kê danh mục
        Task<ThongKeDanhMuc> ThongKeDanhMuc(int beginMonth, int beginYear, int endMonth, int endYear);

        // Thống kê khoảng giá
        Task<ThongKeKhoangGia> ThongKeKhoangGia(int beginMonth, int beginYear, int endMonth, int endYear);
     }
}
