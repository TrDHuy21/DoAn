using Application.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsAdminApiController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsAdminApiController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// Lấy thống kê doanh thu và đơn hàng theo khoảng thời gian
        /// </summary>
        [HttpGet("doanhthuvadonhang")]
        public async Task<IActionResult> GetDoanhThuVaDonHang(
            [FromQuery] int beginMonth = 0,
            [FromQuery] int beginYear = 0,
            [FromQuery] int endMonth = 0,
            [FromQuery] int endYear = 0, 
            [FromQuery] string categoryUrlName = ""
            )
        {
            try
            {
                var result = await _statisticsService.DoanhThuVoiDonHang(
                    beginMonth, beginYear, endMonth, endYear, categoryUrlName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy số lượng đơn hàng theo khoảng thời gian
        /// </summary>
        [HttpGet("soluongdonhang")]
        public async Task<IActionResult> GetSoLuongDonHang(
            [FromQuery] int beginMonth = 0,
            [FromQuery] int beginYear = 0,
            [FromQuery] int endMonth = 0,
            [FromQuery] int endYear = 0)
        {
            try
            {
                var result = await _statisticsService.SoLuongDonHang(
                    beginMonth, beginYear, endMonth, endYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thống kê khách hàng theo khoảng thời gian
        /// </summary>
        [HttpGet("thongkekhachhang")]
        public async Task<IActionResult> GetThongKeKhachHang(
            [FromQuery] int beginMonth = 0,
            [FromQuery] int beginYear = 0,
            [FromQuery] int endMonth = 0,
            [FromQuery] int endYear = 0)
        {
            try
            {
                var result = await _statisticsService.ThongKeKhachHang(
                    beginMonth, beginYear, endMonth, endYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thống kê nhân viên theo khoảng thời gian
        /// </summary>
        [HttpGet("thongkenhanvien")]
        public async Task<IActionResult> GetThongKeNhanVien(
            [FromQuery] int beginMonth = 0,
            [FromQuery] int beginYear = 0,
            [FromQuery] int endMonth = 0,
            [FromQuery] int endYear = 0)
        {
            try
            {
                var result = await _statisticsService.ThongKeNhanVien(
                    beginMonth, beginYear, endMonth, endYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thống kê sản phẩm theo khoảng thời gian
        /// </summary>
        [HttpGet("thongketopsanpham")]
        public async Task<IActionResult> GetThongKeTopSanPham(
            [FromQuery] int beginMonth = 0,
            [FromQuery] int beginYear = 0,
            [FromQuery] int endMonth = 0,
            [FromQuery] int endYear = 0,
            [FromQuery] string categoryUrlName = ""
            )
        {
            try
            {
                var result = await _statisticsService.ThongKeTopSanPham(
                    beginMonth, beginYear, endMonth, endYear, categoryUrlName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thống kê danh mục theo khoảng thời gian
        /// </summary>
        [HttpGet("thongketopdanhmuc")]
        public async Task<IActionResult> GetThongKeTopDanhMuc(
            [FromQuery] int beginMonth = 0,
            [FromQuery] int beginYear = 0,
            [FromQuery] int endMonth = 0,
            [FromQuery] int endYear = 0,
            [FromQuery] int top = -1)
        {
            try
            {
                var result = await _statisticsService.ThongKeTopDanhMuc(
                    beginMonth, beginYear, endMonth, endYear, top);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thống kê khoảng giá theo khoảng thời gian
        /// </summary>
        [HttpGet("thongkekhoanggia")]
        public async  Task<IActionResult> GetThongKeKhoangGia(
            [FromQuery] int beginMonth = 0,
            [FromQuery] int beginYear = 0,
            [FromQuery] int endMonth = 0,
            [FromQuery] int endYear = 0)
        {
            try
            {
                var result = await _statisticsService.ThongKeKhoangGia(
                    beginMonth, beginYear, endMonth, endYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("thongkekhoanggiav2")]
        public async Task<IActionResult> GetThongKeKhoangGiaV2(
            [FromQuery] int beginMonth = 0,
            [FromQuery] int beginYear = 0,
            [FromQuery] int endMonth = 0,
            [FromQuery] int endYear = 0,
            [FromQuery] decimal minPrice = 0,
            [FromQuery] decimal maxPrice = 0,
            [FromQuery] decimal priceStep = 0,
            [FromQuery] string categoryUrlName = ""
            )
        {
            try
            {
                var result = await _statisticsService.ThongKeKhoangGia(
                    beginMonth, beginYear, endMonth, endYear, minPrice, maxPrice, priceStep, categoryUrlName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("thongkedoanhthudanhmuc")]
        public async Task<IActionResult> ThongKeDoanhThuDanhMuc(
            [FromQuery] int beginMonth = 0,
            [FromQuery] int beginYear = 0,
            [FromQuery] int endMonth = 0,
            [FromQuery] int endYear = 0)
        {
            try
            {
                var result = await _statisticsService.ThongKeDoanhThuDanhMuc(
                    beginMonth, beginYear, endMonth, endYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("thongkegiasoluongdoanhso")]
        public async Task<IActionResult> ThongKeGiaSoLuongDoanhSo(
         [FromQuery] int beginMonth = 0,
         [FromQuery] int beginYear = 0,
         [FromQuery] int endMonth = 0,
         [FromQuery] int endYear = 0)
        {
            try
            {
                var result = await _statisticsService.ThongKeGiaSoLuongDoanhSo(
                    beginMonth, beginYear, endMonth, endYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("thongkegiasoluongdoanhsov2")]
        public async Task<IActionResult> ThongKeGiaSoLuongDoanhSov2(
         [FromQuery] int beginMonth = 0,
         [FromQuery] int beginYear = 0,
         [FromQuery] int endMonth = 0,
         [FromQuery] int endYear = 0,
         [FromQuery] decimal minPrice = 0,
         [FromQuery] decimal maxPrice = 0,
         [FromQuery] decimal priceStep = 0,
         [FromQuery] string categoryUrlName = "")
        {
            try
            {
                var result = await _statisticsService.ThongKeGiaSoLuongDoanhSo(
                    beginMonth, beginYear, endMonth, endYear, minPrice, maxPrice, priceStep, categoryUrlName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
