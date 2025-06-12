using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.StatisticsModel;
using Application.Service.Interface;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DoanhThuVaDonHang>> DoanhThuVoiDonHang(int beginMonth, int beginYear, int endMonth, int endYear)
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);
            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            var orders = await _unitOfWork.Orders.GetAll()
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Include(o => o.OrderDetails)
                .ToListAsync();

            var result = new List<DoanhThuVaDonHang>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var monthOrders = orders.Where(o => o.OrderDate.Year == currentDate.Year &&
                                                  o.OrderDate.Month == currentDate.Month);

                result.Add(new DoanhThuVaDonHang
                {
                    Thang = $"{currentDate.Month}/{currentDate.Year}",
                    DoanhThuOnline = monthOrders
                        .Where(o => o.Type == "online")
                        .SelectMany(o => o.OrderDetails)
                        .Sum(od => od.Quantity * od.Price),
                    DoanhThuOffline = monthOrders
                        .Where(o => o.Type == "offline")
                            .SelectMany(o => o.OrderDetails)
                            .Sum(od => od.Quantity * od.Price),
                    SoLuongOffline = monthOrders
                        .Where(o => o.Type == "offline")
                            .SelectMany(o => o.OrderDetails)
                            .Count(),
                    SoLuongOnline = monthOrders
                        .Where(o => o.Type == "offline")
                            .SelectMany(o => o.OrderDetails)
                            .Count(),
                });

                currentDate = currentDate.AddMonths(1);
            }

            return result;
        }

        public async Task<dynamic> SoLuongDonHang(int beginMonth, int beginYear, int endMonth, int endYear)
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);
            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            var orders = await _unitOfWork.Orders.GetAll()
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToListAsync();

            return new
            {
                TongDonHang = orders.Count(),
                DonHangOnline = orders.Count(o => o.Type == "online"),
                DonHangOffline = orders.Count(o => o.Type == "offline")
            };
        }

        public async Task<ThongKeKhachHang> ThongKeKhachHang(int beginMonth, int beginYear, int endMonth, int endYear)
        {
            //var startDate = new DateTime(beginYear, beginMonth, 1);
            //var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            //var orders = await _unitOfWork.Orders.GetAll()
            //    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            //    .Include(o => o.Customer)
            //    .ToListAsync();

            //var uniqueCustomers = orders.Select(o => o.CustomerId).Distinct().Count();
            //var cancelledOrders = orders.Count(o => o.Status == 8);

            //return new ThongKeKhachHang
            //{
            //    TongKhachHang = uniqueCustomers,
            //    KhachHangMoi = orders.Where(o => o.User.CreatedAt >= startDate)
            //                        .Select(o => o.UserId)
            //                        .Distinct()
            //                        .Count(),
            //    KhachHangCu = uniqueCustomers - orders.Where(o => o.User.CreatedAt >= startDate)
            //                                        .Select(o => o.UserId)
            //                                        .Distinct()
            //                                        .Count(),
            //    DonHangBiHuy = cancelledOrders,
            //    TyLeHuy = orders.Any() ? (double)cancelledOrders / orders.Count() * 100 : 0
            //};
            return null;
        }

        public async Task<dynamic> ThongKeNhanVien(int beginMonth, int beginYear, int endMonth, int endYear)
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);

            Dictionary<string, int> labels = new Dictionary<string, int> ();
            int labelsIndex = 0;
            for (int year = beginYear; year <= endYear; year++)
            {
                int startMonth = (year == beginYear) ? beginMonth : 1;
                int endMonthForYear = (year == endYear) ? endMonth : 12;

                for (int month = startMonth; month <= endMonthForYear; month++)
                {
                    labels.Add(($"{month}-{year}"), labelsIndex);
                    labelsIndex++;
                }
            }

            Dictionary<int, ThongKeNhanVien> employees = new Dictionary<int, ThongKeNhanVien>();


            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            var orders = await _unitOfWork.Orders.GetAll()
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Type=="offline")
                .Include(u => u.Employee)
                .Include(o => o.OrderDetails)
                .ToListAsync();

            foreach(var o in orders)
            {
                int month = o.OrderDate.Month;
                int year = o.OrderDate.Year;

                if (!employees.ContainsKey(o.EmployeeId.Value))
                {
                    employees.Add(o.EmployeeId.Value, new ThongKeNhanVien()
                    {
                        TenNhanVien = o.Employee.Username + " - " + o.Employee.Name,
                        DoanhSo = new List<decimal>(new decimal[labels.Count]),
                        DonHang = new List<int>(new int[labels.Count])
                    });

                }

                employees[o.EmployeeId.Value].DoanhSo[labels[$"{month}-{year}"]] += o.OrderDetails.Sum(od => od.Price*od.Quantity);
                employees[o.EmployeeId.Value].DonHang[labels[$"{month}-{year}"]]++;
            }

            return new
            {
                Labels = labels.Keys,
                Employees = employees.Values.ToList(),
            };
        }

        public async Task<ThongKeSanPham> ThongKeTopSanPham(int beginMonth, int beginYear, int endMonth, int endYear)
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);

            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            var orderDetails = await _unitOfWork.OrderDetails.GetAll()
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .Include(od => od.ProductDetail)
                    .ThenInclude(pd => pd.Product)
                .ToListAsync();

            var topProductsByQuantity = orderDetails
                .GroupBy(od => new { od.ProductDetail.ProductId, od.ProductDetail.Product.Name })
                .Select(g => new TopSanPham
                {
                    Id = g.Key.ProductId.Value,
                    Ten = g.Key.Name,
                    SoLuong = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.SoLuong)
                .Take(5)
                .ToList();

            var topProductsByRevenue = orderDetails
                .GroupBy(od => new { od.ProductDetail.ProductId, od.ProductDetail.Product.Name })
                .Select(g => new TopSanPham
                {
                    Id = g.Key.ProductId.Value,
                    Ten = g.Key.Name,
                    DoanhThu = g.Sum(od => od.Quantity * od.Price)
                })
                .OrderByDescending(p => p.DoanhThu)
                .Take(5)
                .ToList();

            return new ThongKeSanPham
            {
                TopSanPhamTheoSoLuong = topProductsByQuantity,
                TopSanPhamTheoDoanhThu = topProductsByRevenue
            };
        }

        public async Task<ThongKeDanhMuc> ThongKeTopDanhMuc(int beginMonth, int beginYear, int endMonth, int endYear)
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);

            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            var orderDetails = await _unitOfWork.OrderDetails.GetAll()
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .Include(od => od.ProductDetail)
                    .ThenInclude(pd => pd.Product)
                        .ThenInclude(p => p.Category)
                .ToListAsync();

            var topCategoriesByQuantity = orderDetails
                .GroupBy(od => new { od.ProductDetail.Product.CategoryId, od.ProductDetail.Product.Category.Name })
                .Select(g => new TopDanhMuc
                {
                    Id = g.Key.CategoryId.Value,
                    Ten = g.Key.Name,
                    SoLuong = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(c => c.SoLuong)
                .Take(3)
                .ToList();

            var topCategoriesByRevenue = orderDetails
                .GroupBy(od => new { od.ProductDetail.Product.CategoryId, od.ProductDetail.Product.Category.Name })
                .Select(g => new TopDanhMuc
                {
                    Id = g.Key.CategoryId.Value,
                    Ten = g.Key.Name,
                    DoanhThu = g.Sum(od => od.Quantity * od.Price)
                })
                .OrderByDescending(c => c.DoanhThu)
                .Take(3)
                .ToList();

            return new ThongKeDanhMuc
            {
                TopDanhMucTheoSoLuong = topCategoriesByQuantity,
                TopDanhMucTheoDoanhThu = topCategoriesByRevenue
            };
        }

        public async Task<ThongKeKhoangGia> ThongKeKhoangGia(int beginMonth, int beginYear, int endMonth, int endYear)
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);

            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            var orderDetails = await _unitOfWork.OrderDetails.GetAll()
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .Include(od => od.ProductDetail)
                .Include(od => od.Order)
                .ToListAsync();

            var priceRanges = new[]
            {
                new { Min = 0, Max = 5000000, Label = "0-5 triệu" },
                new { Min = 5000000, Max = 10000000, Label = "5-10 triệu" },
                new { Min = 10000000, Max = 15000000, Label = "10-15 triệu" },
                new { Min = 15000000, Max = 20000000, Label = "15-20 triệu" },
                new { Min = 20000000, Max = int.MaxValue, Label = "20+ triệu" }
            };

            var monthlyData = new List<Dictionary<string, int>>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var monthOrders = orderDetails.Where(od =>
                    od.Order.OrderDate.Year == currentDate.Year &&
                    od.Order.OrderDate.Month == currentDate.Month);

                var monthData = new Dictionary<string, int>();
                foreach (var range in priceRanges)
                {
                    monthData[range.Label] = monthOrders
                        .Count(od => od.Price >= range.Min && od.Price < range.Max);
                }

                monthlyData.Add(monthData);
                currentDate = currentDate.AddMonths(1);
            }

            var totalByRange = priceRanges.Select(range => new
            {
                Range = range.Label,
                Percentage = orderDetails.Any() ?
                    (double)orderDetails.Count(od => od.Price >= range.Min && od.Price < range.Max) /
                    orderDetails.Count * 100 : 0
            }).ToList();

            return new ThongKeKhoangGia
            {
                TyLeTheoKhoangGia = totalByRange.ToDictionary(x => x.Range, x => x.Percentage),
                DuLieuTheoThang = monthlyData
            };
        }

        private void ValidateAndControlTime(ref int beginMonth, ref int beginYear, ref int endMonth, ref int endYear)
        {

            // Lấy năm và tháng hiện tại
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;

            // Thiết lập min và max
            int minYear = 2024;
            int minMonth = 1; // Tháng 1
            int maxYear = currentYear;
            int maxMonth = currentMonth;

            if (beginMonth < 1 || beginMonth > 12 || endMonth < 1 || endMonth > 12 ||
              endYear < beginYear || endYear <= 0 || beginYear <= 0)
            {
                // Trường hợp số âm, số tháng không hợp lệ
                beginMonth = 1;
                beginYear = currentYear;
                endMonth = currentMonth;
                endYear = currentYear;
                return;
            }

            if (endYear < minYear)
            {
                endYear = minYear;
                endMonth = minMonth;
            }

            if (beginYear < minYear)
            {
                beginYear = minYear;
                beginMonth = minMonth;
            }

            if (beginYear == minYear && beginMonth < minMonth)
            {
                beginYear = minYear;
                beginMonth = minMonth;
            }

            if (endYear > maxYear)
            {
                endMonth = maxMonth;
                endYear = maxYear;
            }
            if (endYear == maxYear && endMonth >= maxMonth)
            {
                endMonth = maxMonth;
                endYear = maxYear;
            }
        }

    }
}
