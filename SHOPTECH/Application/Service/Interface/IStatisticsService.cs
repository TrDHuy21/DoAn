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
        Task<List<DoanhThuVaDonHang>> DoanhThuVoiDonHang(int beginMonth, int beginYear, int endMonth, int endYear, string categoryUrlName="");
        Task<dynamic> SoLuongDonHang(int beginMonth, int beginYear, int endMonth, int endYear);

        // Thống kê khách hàng
        Task<ThongKeKhachHang> ThongKeKhachHang(int beginMonth, int beginYear, int endMonth, int endYear);

        // Thống kê nhân viên
        Task<dynamic> ThongKeNhanVien(int beginMonth, int beginYear, int endMonth, int endYear);

        // Thống kê sản phẩm
        Task<ThongKeSanPham> ThongKeTopSanPham(int beginMonth, int beginYear, int endMonth, int endYear, string categoryUrlName = "");

        // Thống kê danh mục
        Task<ThongKeDanhMuc> ThongKeTopDanhMuc(int beginMonth, int beginYear, int endMonth, int endYear, int top);

        // Thống kê khoảng giá
        Task<ThongKeKhoangGia> ThongKeKhoangGia(int beginMonth, int beginYear, int endMonth, int endYear);
        Task<ThongKeKhoangGia> ThongKeKhoangGia(
            int beginMonth,
            int beginYear,
            int endMonth,
            int endYear,
            decimal minPrice,
            decimal maxPrice,
            decimal priceStep,
            string categoryUrlName);

        // thông kê doanh thu theo theo danh mục
        Task<ThongKeDanhMuc> ThongKeDoanhThuDanhMuc(int beginMonth, int beginYear, int endMonth, int endYear);

        // thông kế giá, số lượng, doanh số.
        Task<List<ThongKeSanPhamKhoangGia>> ThongKeGiaSoLuongDoanhSo(int beginMonth, int beginYear, int endMonth, int endYear);
        Task<List<ThongKeSanPhamKhoangGia>> ThongKeGiaSoLuongDoanhSo(
            int beginMonth,
            int beginYear,
            int endMonth,
            int endYear,
            decimal minPrice,
            decimal maxPrice,
            decimal priceStep,
            string categoryUrlName);
    }
}
