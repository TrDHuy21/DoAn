using System;
using System.Collections;
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

        public async Task<List<DoanhThuVaDonHang>> DoanhThuVoiDonHang(int beginMonth, int beginYear, int endMonth, int endYear, string categoryUrlName = "")
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);
            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            var orders = await _unitOfWork.Orders.GetAll()
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .SelectMany(o => o.OrderDetails)
                .Where(od => categoryUrlName == "" ? true : od.ProductDetail.Product.Category.UrlName == categoryUrlName)
                .Select(od => new {
                    OrderDate = od.Order.OrderDate,
                    Type = od.Order.Type,
                    Quantity = od.Quantity,
                    Price = od.Price,
                })
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
                        .Sum(od => od.Quantity * od.Price),
                    DoanhThuOffline = monthOrders
                        .Where(o => o.Type == "offline")
                        .Sum(od => od.Quantity * od.Price),
                    SoLuongOffline = monthOrders
                        .Where(o => o.Type == "offline")
                        .Count(),
                    SoLuongOnline = monthOrders
                        .Where(o => o.Type == "online")
                        .Count(),
                });

                currentDate = currentDate.AddMonths(1);
            }

            return result;
        }

        private bool checkCategoryOfProductDetail(int productDetailId, string categoryUrlName)
        {
            if (string.IsNullOrEmpty(categoryUrlName))
            {
                return true; // Không lọc theo danh mục
            }
            var category = _unitOfWork.ProductDetails.GetAll().Where(pd => pd.Id == productDetailId)
                .Select(pd => pd.Product.Category.UrlName).AsNoTracking().First();
            return category == categoryUrlName;
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

            Dictionary<string, int> labels = new Dictionary<string, int>();
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
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Type == "offline")
                .Include(u => u.Employee)
                .Include(o => o.OrderDetails)
                .ToListAsync();

            foreach (var o in orders)
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

                employees[o.EmployeeId.Value].DoanhSo[labels[$"{month}-{year}"]] += o.OrderDetails.Sum(od => od.Price * od.Quantity);
                employees[o.EmployeeId.Value].DonHang[labels[$"{month}-{year}"]]++;
            }

            return new
            {
                Labels = labels.Keys,
                Employees = employees.Values.ToList(),
            };
        }

        public async Task<ThongKeSanPham> ThongKeTopSanPham(int beginMonth, int beginYear, int endMonth, int endYear, string categoryUrlName ="")
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);

            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            var orderDetails = await _unitOfWork.OrderDetails.GetAll()
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .Where(od => categoryUrlName == "" ? true : od.ProductDetail.Product.Category.UrlName == categoryUrlName)
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

        public async Task<ThongKeDanhMuc> ThongKeTopDanhMuc(int beginMonth, int beginYear, int endMonth, int endYear, int top = -1)
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
                .ToList();

            if (top != -1)
            {
                topCategoriesByRevenue = topCategoriesByRevenue.Take(top).ToList();
                topCategoriesByQuantity = topCategoriesByQuantity.Take(top).ToList();
            }

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
        public async Task<ThongKeKhoangGia> ThongKeKhoangGia(
            int beginMonth,
            int beginYear,
            int endMonth,
            int endYear,
            decimal minPrice = 0,
            decimal maxPrice = 0,
            decimal priceStep = 0,
            string categoryUrlName = "")
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);
            if (maxPrice <= 0)
            {
                maxPrice = await _unitOfWork.OrderDetails.GetAll()
                    .MaxAsync(od => od.Price);
            }
            priceStep = priceStep == 0 ? 2500000 : priceStep;
            // Validate price parameters
            if (minPrice < 0 || maxPrice <= minPrice || priceStep <= 0)
            {
                throw new ArgumentException("Invalid price parameters. Ensure minPrice >= 0, maxPrice > minPrice, and priceStep > 0");
            }

            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            var orderDetails = await _unitOfWork.OrderDetails.GetAll()
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .Where(od => categoryUrlName == "" ? true : od.ProductDetail.Product.Category.UrlName == categoryUrlName)
                .Include(od => od.ProductDetail)
                .Include(od => od.Order)
                .ToListAsync();

            // Generate dynamic price ranges based on input parameters
            var priceRanges = new List<dynamic>();
            decimal currentMin = minPrice;

            while (currentMin < maxPrice)
            {
                decimal currentMax = Math.Min(currentMin + priceStep, maxPrice);

                string label;

                label = $"{FormatPriceToTrieu(currentMin)}-{FormatPriceToTrieu(currentMin + priceStep)} triệu";

                priceRanges.Add(new
                {
                    Min = currentMin,
                    Max = currentMin + priceStep,
                    Label = label,
                    IsLast = false
                });
                currentMin += priceStep;
            }

            priceRanges.Add(new
            {
                Min = currentMin,
                Max = currentMin + priceStep,
                Label = $"{FormatPriceToTrieu(currentMin)}+ triệu",
                IsLast = true
            });

            // Generate monthly data
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
                        .Count(od =>
                            od.Price >= range.Min &&
                            (range.IsLast ? od.Price <= range.Max : od.Price < range.Max));
                }

                monthlyData.Add(monthData);
                currentDate = currentDate.AddMonths(1);
            }

            // Calculate percentage by range
            var totalByRange = priceRanges.Select(range => new
            {
                Range = (string)range.Label,
                Percentage = orderDetails.Any() ?
                    (double)orderDetails.Count(od =>
                        od.Price >= range.Min &&
                        (range.IsLast ? od.Price <= range.Max : od.Price < range.Max)) /
                    orderDetails.Count * 100 : 0
            }).ToList();

            return new ThongKeKhoangGia
            {
                TyLeTheoKhoangGia = totalByRange.ToDictionary(x => x.Range, x => x.Percentage),
                DuLieuTheoThang = monthlyData
            };
        }

        public async Task<List<ThongKeSanPhamKhoangGia>> ThongKeSanPhamTheoKhoangGia(int beginMonth, int beginYear, int endMonth, int endYear)
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);

            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            // Define price ranges in millions
            var priceRanges = new[]
            {
                new { Min = 0, Max = 5000000, Label = "0-5 triệu" },
                new { Min = 5000000, Max = 10000000, Label = "5-10 triệu" },
                new { Min = 10000000, Max = 15000000, Label = "10-15 triệu" },
                new { Min = 15000000, Max = 20000000, Label = "15-20 triệu" },
                new { Min = 20000000, Max = int.MaxValue, Label = "20+ triệu" }
            };

            // Get all products and their sales data
            var products = await _unitOfWork.Products.GetAll()
                .Include(p => p.ProductDetails)
                .ToListAsync();

            var orderDetails = await _unitOfWork.OrderDetails.GetAll()
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .Include(od => od.ProductDetail)
                .ToListAsync();

            var result = new List<ThongKeSanPhamKhoangGia>();

            foreach (var range in priceRanges)
            {
                // Get products in this price range
                var productsInRange = products
                    .Where(p => p.ProductDetails.Any(pd => pd.Price >= range.Min && pd.Price < range.Max))
                    .ToList();

                // Get sales data for products in this range
                var salesInRange = orderDetails
                    .Where(od => od.Price >= range.Min && od.Price < range.Max)
                    .ToList();

                // Calculate statistics
                var totalProducts = productsInRange.Count;
                var totalSold = salesInRange.Sum(od => od.Quantity);
                var averagePrice = salesInRange.Any()
                    ? salesInRange.Average(od => od.Price)
                    : 0;

                result.Add(new ThongKeSanPhamKhoangGia
                {
                    KhoangGia = range.Label,
                    GiaTrungBinh = averagePrice,
                    TongSoLoaiSanPham = totalProducts,
                    TongSoSanPhamBanDuoc = totalSold
                });
            }

            return result;
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
              endYear < beginYear || endYear <= 0 || beginYear <= 0 || (endYear == beginYear && beginMonth > endMonth))
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

        public async Task<ThongKeDanhMuc> ThongKeDoanhThuDanhMuc(int beginMonth, int beginYear, int endMonth, int endYear)
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);

            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            // Get all order details within the time period with related product and category information
            var orderDetails = await _unitOfWork.OrderDetails.GetAll()
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .Include(od => od.ProductDetail)
                    .ThenInclude(pd => pd.Product)
                        .ThenInclude(p => p.Category)
                .ToListAsync();

            // Calculate revenue for each category
            var categoryRevenue = orderDetails
                .GroupBy(od => new
                {
                    CategoryId = od.ProductDetail.Product.CategoryId,
                    CategoryName = od.ProductDetail.Product.Category.Name
                })
                .Select(g => new TopDanhMuc
                {
                    Id = g.Key.CategoryId.Value,
                    Ten = g.Key.CategoryName,
                    DoanhThu = g.Sum(od => od.Quantity * od.Price)
                })
                .OrderByDescending(c => c.DoanhThu)
                .ToList();

            // Calculate total revenue across all categories
            decimal totalRevenue = categoryRevenue.Sum(c => c.DoanhThu);

            return new ThongKeDanhMuc
            {
                TopDanhMucTheoDoanhThu = categoryRevenue,
            };
        }

        public async Task<List<ThongKeSanPhamKhoangGia>> ThongKeGiaSoLuongDoanhSo(int beginMonth, int beginYear, int endMonth, int endYear)
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);

            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            // Define price ranges in millions
            var priceRanges = new[]
            {
                new { Min = 0, Max = 5000000, Label = "0-5 triệu" },
                new { Min = 5000000, Max = 10000000, Label = "5-10 triệu" },
                new { Min = 10000000, Max = 15000000, Label = "10-15 triệu" },
                new { Min = 15000000, Max = 20000000, Label = "15-20 triệu" },
                new { Min = 20000000, Max = int.MaxValue, Label = "20+ triệu" }
            };

            // Get all products and their sales data
            var products = await _unitOfWork.Products.GetAll()
                .Include(p => p.ProductDetails)
                .ToListAsync();

            var orderDetails = await _unitOfWork.OrderDetails.GetAll()
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .Include(od => od.ProductDetail)
                .ToListAsync();

            var result = new List<ThongKeSanPhamKhoangGia>();

            foreach (var range in priceRanges)
            {
                // Get products in this price range
                var productsInRange = products
                    .Where(p => p.ProductDetails.Any(pd => pd.Price >= range.Min && pd.Price < range.Max))
                    .ToList();

                // Get sales data for products in this range
                var salesInRange = orderDetails
                    .Where(od => od.Price >= range.Min && od.Price < range.Max)
                    .ToList();

                // Calculate statistics
                var totalProducts = productsInRange.Count;
                var totalSold = salesInRange.Sum(od => od.Quantity);
                var averagePrice = salesInRange.Any()
                    ? salesInRange.Average(od => od.Price)
                    : 0;

                result.Add(new ThongKeSanPhamKhoangGia
                {
                    KhoangGia = range.Label,
                    GiaTrungBinh = Math.Round(averagePrice),
                    TongSoLoaiSanPham = totalProducts,
                    TongSoSanPhamBanDuoc = totalSold
                });
            }

            return result;
        }


        public async Task<List<ThongKeSanPhamKhoangGia>> ThongKeGiaSoLuongDoanhSo(
            int beginMonth,
            int beginYear,
            int endMonth,
            int endYear,
            decimal minPrice = 0,
            decimal maxPrice = 0,
            decimal priceStep = 0,
            string categoryUrlName = "")
        {
            ValidateAndControlTime(ref beginMonth, ref beginYear, ref endMonth, ref endYear);
            if (maxPrice <= 0)
            {
                maxPrice = await _unitOfWork.OrderDetails.GetAll()
                    .MaxAsync(od => od.Price);
            }
            priceStep = priceStep == 0 ? 2500000 : priceStep;
            // Validate price parameters
            if (minPrice < 0 || maxPrice <= minPrice || priceStep <= 0)
            {
                throw new ArgumentException("Invalid price parameters. Ensure minPrice >= 0, maxPrice > minPrice, and priceStep > 0");
            }

            var startDate = new DateTime(beginYear, beginMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1).AddMonths(1).AddDays(-1);

            // Generate dynamic price ranges based on input parameters
            var priceRanges = new List<dynamic>();
            decimal currentMin = minPrice;

            while (currentMin < maxPrice)
            {
                decimal currentMax = Math.Min(currentMin + priceStep, maxPrice);

                string label;

                label = $"{FormatPriceToTrieu(currentMin)}-{FormatPriceToTrieu(currentMin + priceStep)} triệu";

                priceRanges.Add(new
                {
                    Min = currentMin,
                    Max = currentMin + priceStep,
                    Label = label,
                    IsLast = false
                });
                currentMin += priceStep;
            }

            priceRanges.Add(new
            {
                Min = currentMin,
                Max = currentMin + priceStep,
                Label = $"{FormatPriceToTrieu(currentMin)}+ triệu",
                IsLast = true
            });


            // Get all products and their sales data
            var products = await _unitOfWork.Products.GetAll()
                .Where(p => categoryUrlName == "" ? true : p.Category.UrlName == categoryUrlName)
                .Include(p => p.ProductDetails)
                .ToListAsync();

            var orderDetails = await _unitOfWork.OrderDetails.GetAll()
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .Where(od => categoryUrlName == "" ? true : od.ProductDetail.Product.Category.UrlName == categoryUrlName)
                .Include(od => od.ProductDetail)
                .ToListAsync();

            var result = new List<ThongKeSanPhamKhoangGia>();

            foreach (var range in priceRanges)
            {
                // Get products in this price range
                var productsInRange = products
                    .Where(p => p.ProductDetails.Any(pd =>
                        pd.Price >= range.Min &&
                        (range.IsLast ? pd.Price <= range.Max : pd.Price < range.Max)))
                    .ToList();

                // Get sales data for products in this range
                var salesInRange = orderDetails
                    .Where(od =>
                        od.Price >= range.Min &&
                        (range.IsLast ? od.Price <= range.Max : od.Price < range.Max))
                    .ToList();

                // Calculate statistics
                var totalProducts = productsInRange.Count;
                var totalSold = salesInRange.Sum(od => od.Quantity);
                var averagePrice = salesInRange.Any()
                    ? salesInRange.Average(od => od.Price)
                    : 0;

                result.Add(new ThongKeSanPhamKhoangGia
                {
                    KhoangGia = range.Label,
                    GiaTrungBinh = Math.Round(averagePrice),
                    TongSoLoaiSanPham = totalProducts,
                    TongSoSanPhamBanDuoc = totalSold
                });
            }

            return result;
        }

        // Helper method to format price to "triệu" format
        private string FormatPriceToTrieu(decimal price)
        {
            if (price >= 1000000)
            {
                decimal trieu = price / 1000000;
                if (trieu == Math.Floor(trieu))
                {
                    return trieu.ToString("0");
                }
                else
                {
                    return trieu.ToString("0.#");
                }
            }
            else if (price >= 1000)
            {
                return $"{price / 1000}k";
            }
            else
            {
                return price.ToString("0");
            }
        }
    }
}
