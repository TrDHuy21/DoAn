using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.OrderDtos;
using Application.Models;
using Application.Models.EmailModel;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X9;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<Order> CreateOfflineOrder(CreateOfflineOrderDto createOfflineOrderDto)
        {
            try
            {
                var order = _mapper.Map<Order>(createOfflineOrderDto);

                if (createOfflineOrderDto.OrderDetails.Count() == 0)
                {
                    throw new Exception("Danh sách sản phẩm rỗng");
                }

                var user = _httpContextAccessor.HttpContext?.User;
                int userId = Convert.ToInt32(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new Exception("Lỗi trong quá trình lấy thông tin người dùng"));

                order.EmployeeId = userId;
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangeAsync();

                foreach (var item in createOfflineOrderDto.OrderDetails)
                {
                    var productDetail = await _unitOfWork.ProductDetails.GetByIdAsync(item.ProductDetailId)
                          ?? throw new Exception("Lỗi trong quá trình lấy sản phẩm");
                    if (item.Quantity <= productDetail.Quantity)
                    {
                        var orderDetail = new OrderDetail()
                        {
                            OrderId = order.Id,
                            ProductDetailId = productDetail.Id,
                            Price = productDetail.Price,
                            Quantity = item.Quantity,
                        };
                        productDetail.Quantity -= item.Quantity;

                        await _unitOfWork.OrderDetails.AddAsync(orderDetail);
                    }
                    else
                    {
                        throw new Exception("Số lượng kho không đủ");
                    }
                }
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }

        }

        public async Task<Order> CreateOnlineOrder(CreateOnlineOrderDto createOrderOnlineDto)
        {
            try
            {
                var order = _mapper.Map<Order>(createOrderOnlineDto);

                if (createOrderOnlineDto.OrderDetails.Count() == 0)
                {
                    throw new Exception("Danh sách sản phẩm rỗng");
                }

                var user = _httpContextAccessor.HttpContext?.User;
                int userId = Convert.ToInt32(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new Exception("Lỗi trong quá trình lấy thông tin người dùng"));

                order.CustomerId = userId;
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangeAsync();

                foreach (var item in createOrderOnlineDto.OrderDetails)
                {
                    var productDetail = await _unitOfWork.ProductDetails.GetByIdAsync(item.ProductDetailId)
                          ?? throw new Exception("Lỗi trong quá trình lấy sản phẩm");
                    if (item.Quantity <= productDetail.Quantity)
                    {
                        var orderDetail = new OrderDetail()
                        {
                            OrderId = order.Id,
                            ProductDetailId = productDetail.Id,
                            Price = productDetail.Price,
                            Quantity = item.Quantity,
                        };
                        productDetail.Quantity -= item.Quantity;

                        await _unitOfWork.OrderDetails.AddAsync(orderDetail);
                    }
                    else
                    {
                        throw new Exception("Số lượng kho không đủ");
                    }
                }
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task<IEnumerable<Order>> GetAll(int? status)
        {
            // gett all order
            var orders = await _unitOfWork.Orders.GetAll()
                .Include(x => x.Customer)
                .Include(x => x.Employee)
                .Include(x => x.Ward)
                .ThenInclude(x => x.District)
                .ThenInclude(x => x.Province)
                .Include(x => x.OrderDetails)
                .Where(x => status == null ? true : status == x.Status)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<Order>> GetAllByCustomerId(int customerId)
        {
            var orders = await _unitOfWork.Orders.GetAll()
                .Include(x => x.Customer)
                .Include(x => x.Employee)
                .Include(x => x.Ward)
                .ThenInclude(x => x.District)
                .ThenInclude(x => x.Province)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.ProductDetail)
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<Order>> GetAllByEmployeeId(int employeeId)
        {
            // get order by user id
            var orders = await _unitOfWork.Orders.GetAll()
                .Include(x => x.Customer)
                .Include(x => x.Employee)
                .Include(x => x.Ward)
                .ThenInclude(x => x.District)
                .ThenInclude(x => x.Province)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.ProductDetail)
                .Where(x => x.EmployeeId == employeeId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return orders;
        }

        public async Task<Order> GetById(int id)
        {
            // check role of use: if customer then check order buy by customer id
            // if employee and admin then can check all order

            // get user id, user role
            var user = _httpContextAccessor.HttpContext?.User;
            int userId = Convert.ToInt32(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("Lỗi trong quá trình lấy thông tin người dùng"));
            string role = user?.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new Exception("Lỗi trong quá trình lấy thông tin người dùng");

            // get by order id
            var order = await _unitOfWork.Orders.GetAll()
            .Include(x => x.Customer)
            .Include(x => x.Employee)
            .Include(x => x.Ward)
            .ThenInclude(x => x.District)
            .ThenInclude(x => x.Province)
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.ProductDetail)
            .ThenInclude(pd => pd.Image)
            .FirstOrDefaultAsync(x => x.Id == id && (x.CustomerId == userId || role == "Admin" || role == "Saler"))
            ?? throw new Exception("Not found");

            return order;
        }

        public async Task<IEnumerable<Order>> GetMyOrder()
        {
            // get user id, user role
            var user = _httpContextAccessor.HttpContext?.User;
            int userId = Convert.ToInt32(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("Lỗi trong quá trình lấy thông tin người dùng"));
            string role = user?.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new Exception("Lỗi trong quá trình lấy thông tin người dùng");

            if (role == "Customer")
            {
                return await GetAllByCustomerId(userId);
            }
            else if (role == "Saler")
            {
                return await GetAllByEmployeeId(userId);
            }
            else
            {
                throw new Exception("Lỗi trong quá trình lấy thông tin người dùng");
            }
        }

        public async Task SendNewOrderNotificationForAdminAsync(Order order)
        {
            var customer = await _unitOfWork.Users.GetByIdAsync(order.CustomerId ?? 0);
            var subject = $"🛒 Đơn hàng #{order.Id} mới từ {customer?.Name}";

            var body = await DisplayHtml(order);

            // get all email admin
            var admins = await _unitOfWork.Users.GetAll()
                .Where(x => x.RoleId == 1)
                .ToListAsync();
            var emailMessage = new EmailMessage
            {
                Recipients = admins
                .Where(e => !string.IsNullOrEmpty(e.Email))
                .Select(e => new EmailRecipient()
                {
                    Email = e.Email,
                    Name = e.Name,
                    Type = RecipientType.To
                }).ToList(),
                Subject = subject,
                Body = body,
                Priority = 1 // High priority
            };
            await _emailService.SendEmailAsync(emailMessage);
        }

        public async Task SendNewOrderNotificationForCustomerAsync(Order order)
        {
            var customer = await _unitOfWork.Users.GetByIdAsync(order.CustomerId ?? 0);
            if (customer?.Email == null)
            {
                return;
            }
            var subject = $"🛒 Đơn hàng #{order.Id} mới từ {customer?.Name}";

            var body = await DisplayHtml(order);

            var emailMessage = new EmailMessage
            {
                Recipients = new List<EmailRecipient>
                {
                    new EmailRecipient
                    {
                        Email = customer.Email,
                        Name = customer.Name,
                        Type = RecipientType.To
                    }
                },
                Subject = subject,
                Body = body,
                Priority = 1 // High priority
            };
            await _emailService.SendEmailAsync(emailMessage);
        }

        public async Task SendOrderStatusForCustomerAsync(Order order)
        {
            var customer = await _unitOfWork.Users.GetByIdAsync(order.CustomerId ?? 0);
            if (customer?.Email == null)
            {
                return;
            }
            var TotalAmount = await _unitOfWork.OrderDetails.GetAll()
               .Where(x => x.OrderId == order.Id)
               .SumAsync(x => x.Price * x.Quantity);

            var statusInfo = GetOrderStatusInfo(order.Status);
            var body = $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #ddd; border-radius: 8px; overflow: hidden;'>
                <div style='background: {statusInfo.BackgroundColor}; color: white; padding: 20px; text-align: center;'>
                    <h2>{statusInfo.Icon} {statusInfo.Title}</h2>
                </div>
                <div style='padding: 20px;'>
                    <div style='background: #f8f9fa; padding: 15px; border-radius: 5px; margin-bottom: 20px;'>
                        <h3 style='margin: 0 0 10px 0; color: #333;'>Thông tin đơn hàng</h3>
                        <table style='width: 100%; border-collapse: collapse;'>
                            <tr>
                                <td style='padding: 8px 0; font-weight: bold; width: 35%;'>Mã đơn hàng:</td>
                                <td style='padding: 8px 0;'>{order.Id}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px 0; font-weight: bold;'>Khách hàng:</td>
                                <td style='padding: 8px 0;'>{order.Customer.Name}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px 0; font-weight: bold;'>Tổng tiền:</td>
                                <td style='padding: 8px 0; color: #28a745; font-weight: bold;'>{TotalAmount:N0} VNĐ</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px 0; font-weight: bold;'>Địa chỉ giao hàng:</td>
                                <td style='padding: 8px 0;'>{order.Address}</td>
                            </tr>
                        </table>
                    </div>
                    
                    <div style='background: {statusInfo.MessageBackgroundColor}; border-left: 4px solid {statusInfo.BackgroundColor}; padding: 15px; border-radius: 0 5px 5px 0;'>
                        <h4 style='margin: 0 0 10px 0; color: {statusInfo.BackgroundColor};'>Trạng thái hiện tại</h4>
                        <p style='margin: 0; font-size: 16px; color: #333;'>{statusInfo.Message}</p>
                    </div>
                    
                    {statusInfo.AdditionalInfo}
                    
                    <div style='margin-top: 20px; padding: 15px; background: #e9ecef; border-radius: 5px; text-align: center;'>
                        <p style='margin: 0; color: #666; font-size: 14px;'>
                            Nếu bạn có bất kỳ thắc mắc nào, vui lòng liên hệ với chúng tôi qua hotline: <strong>1900-xxxx</strong>
                        </p>
                    </div>
                </div>
                <div style='background: #f8f9fa; padding: 15px; text-align: center; border-top: 1px solid #ddd;'>
                    <p style='margin: 0; color: #666; font-size: 12px;'>
                        Email này được gửi tự động từ hệ thống. Vui lòng không trả lời email này.
                    </p>
                </div>
            </div>";

            await _emailService.SendSimpleEmailAsync(order.Customer.Email, statusInfo.Subject, body);
        }

        private async Task<string> DisplayHtml(Order order)
        {
            var customer = await _unitOfWork.Users.GetByIdAsync(order.CustomerId ?? 0);
            var TotalAmount = await _unitOfWork.OrderDetails.GetAll()
                .Where(x => x.OrderId == order.Id)
                .SumAsync(x => x.Price * x.Quantity);


            var html = $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                <div style='background: #007bff; color: white; padding: 20px; text-align: center;'>
                    <h2>🛒 ĐỚN HÀNG MỚI</h2>
                </div>
                <div style='padding: 20px; border: 1px solid #ddd;'>
                    <table style='width: 100%; border-collapse: collapse;'>
                        <tr>
                            <td style='padding: 10px; font-weight: bold;'>Mã đơn hàng:</td>
                            <td style='padding: 10px;'>{order.Id}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; font-weight: bold;'>Khách hàng:</td>
                            <td style='padding: 10px;'>{order?.Customer?.Name}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; font-weight: bold;'>Tổng tiền:</td>
                            <td style='padding: 10px; color: #28a745; font-weight: bold;'>{TotalAmount:N0} VNĐ</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; font-weight: bold;'>Thời gian:</td>
                            <td style='padding: 10px;'>{DateTime.Now:dd/MM/yyyy HH:mm:ss}</td>
                        </tr>
                    </table>
                </div>
            </div>";
            return html;
        }
        private static OrderStatusInfo GetOrderStatusInfo(int status)
        {
            return status switch
            {
                0 => new OrderStatusInfo
                {
                    Title = "Chờ Xác Nhận",
                    Subject = "Đơn hàng đang chờ xác nhận",
                    Icon = "⏳",
                    BackgroundColor = "#ffc107",
                    MessageBackgroundColor = "#fff3cd",
                    Message = "Đơn hàng của bạn đã được tiếp nhận và đang chờ xác nhận từ cửa hàng. Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.",
                    AdditionalInfo = "<div style='margin: 15px 0; padding: 10px; background: #d1ecf1; border-radius: 5px;'><strong>Lưu ý:</strong> Thời gian xác nhận thông thường từ 1-2 giờ trong giờ hành chính.</div>"
                },
                1 => new OrderStatusInfo
                {
                    Title = "Đã Xác Nhận",
                    Subject = "Đơn hàng đã được xác nhận",
                    Icon = "✅",
                    BackgroundColor = "#28a745",
                    MessageBackgroundColor = "#d4edda",
                    Message = "Tuyệt vời! Đơn hàng của bạn đã được xác nhận và đang được chuẩn bị. Chúng tôi sẽ sớm giao hàng đến bạn.",
                    AdditionalInfo = "<div style='margin: 15px 0; padding: 10px; background: #d4edda; border-radius: 5px;'><strong>Tiếp theo:</strong> Đơn hàng sẽ được đóng gói và chuyển cho đơn vị vận chuyển.</div>"
                },
                2 => new OrderStatusInfo
                {
                    Title = "Đang Giao Hàng",
                    Subject = "Đơn hàng đang được giao đến bạn",
                    Icon = "🚚",
                    BackgroundColor = "#17a2b8",
                    MessageBackgroundColor = "#d1ecf1",
                    Message = "Đơn hàng của bạn đang trên đường giao đến địa chỉ đã đăng ký. Vui lòng chuẩn bị nhận hàng.",
                    AdditionalInfo = "<div style='margin: 15px 0; padding: 10px; background: #d1ecf1; border-radius: 5px;'><strong>Thời gian dự kiến:</strong> Hàng sẽ được giao trong 1-3 ngày làm việc tùy theo khu vực.</div>"
                },
                3 => new OrderStatusInfo
                {
                    Title = "Giao Hàng Thành Công",
                    Subject = "Đơn hàng đã được giao thành công",
                    Icon = "📦",
                    BackgroundColor = "#28a745",
                    MessageBackgroundColor = "#d4edda",
                    Message = "Đơn hàng đã được giao thành công! Cảm ơn bạn đã mua hàng tại cửa hàng của chúng tôi.",
                    AdditionalInfo = "<div style='margin: 15px 0; padding: 10px; background: #d4edda; border-radius: 5px;'><strong>Đánh giá:</strong> Hãy để lại đánh giá về sản phẩm để giúp chúng tôi cải thiện dịch vụ nhé!</div>"
                },
                4 => new OrderStatusInfo
                {
                    Title = "Đơn Hàng Thành Công",
                    Subject = "Đơn hàng hoàn tất thành công",
                    Icon = "🎉",
                    BackgroundColor = "#28a745",
                    MessageBackgroundColor = "#d4edda",
                    Message = "Đơn hàng của bạn đã hoàn tất thành công! Cảm ơn bạn đã tin tưởng và lựa chọn sản phẩm của chúng tôi.",
                    AdditionalInfo = "<div style='margin: 15px 0; padding: 10px; background: #d4edda; border-radius: 5px;'><strong>Hỗ trợ:</strong> Nếu có vấn đề gì với sản phẩm, vui lòng liên hệ với chúng tôi trong vòng 7 ngày.</div>"
                },
                5 => new OrderStatusInfo
                {
                    Title = "Giao Hàng Không Thành Công",
                    Subject = "Giao hàng không thành công - Cần hỗ trợ",
                    Icon = "❌",
                    BackgroundColor = "#dc3545",
                    MessageBackgroundColor = "#f8d7da",
                    Message = "Rất tiếc, việc giao hàng không thành công. Có thể do không liên lạc được với bạn hoặc địa chỉ không chính xác.",
                    AdditionalInfo = "<div style='margin: 15px 0; padding: 10px; background: #f8d7da; border-radius: 5px;'><strong>Hành động:</strong> Vui lòng liên hệ hotline để sắp xếp lại lịch giao hàng hoặc cập nhật thông tin.</div>"
                },
                6 => new OrderStatusInfo
                {
                    Title = "Đang Hoàn Hàng",
                    Subject = "Đơn hàng đang được hoàn trả",
                    Icon = "↩️",
                    BackgroundColor = "#fd7e14",
                    MessageBackgroundColor = "#fff3cd",
                    Message = "Đơn hàng của bạn đang được xử lý hoàn trả theo yêu cầu. Chúng tôi sẽ thông báo khi hoàn tất.",
                    AdditionalInfo = "<div style='margin: 15px 0; padding: 10px; background: #fff3cd; border-radius: 5px;'><strong>Thời gian:</strong> Quá trình hoàn hàng thường mất 3-7 ngày làm việc.</div>"
                },
                7 => new OrderStatusInfo
                {
                    Title = "Hoàn Hàng Thành Công",
                    Subject = "Hoàn hàng đã được xử lý thành công",
                    Icon = "✅",
                    BackgroundColor = "#28a745",
                    MessageBackgroundColor = "#d4edda",
                    Message = "Hoàn hàng đã được xử lý thành công. Số tiền sẽ được hoàn trả theo phương thức thanh toán ban đầu.",
                    AdditionalInfo = "<div style='margin: 15px 0; padding: 10px; background: #d4edda; border-radius: 5px;'><strong>Hoàn tiền:</strong> Tiền sẽ được chuyển về tài khoản trong 1-3 ngày làm việc.</div>"
                },
                8 => new OrderStatusInfo
                {
                    Title = "Đã Hủy",
                    Subject = "Đơn hàng đã được hủy",
                    Icon = "🚫",
                    BackgroundColor = "#6c757d",
                    MessageBackgroundColor = "#f8f9fa",
                    Message = "Đơn hàng đã được hủy theo yêu cầu. Nếu đã thanh toán, số tiền sẽ được hoàn trả.",
                    AdditionalInfo = "<div style='margin: 15px 0; padding: 10px; background: #f8f9fa; border-radius: 5px;'><strong>Mua lại:</strong> Bạn có thể đặt lại đơn hàng bất cứ lúc nào trên website của chúng tôi.</div>"
                },
                _ => new OrderStatusInfo
                {
                    Title = "Trạng Thái Không Xác Định",
                    Subject = "Cập nhật trạng thái đơn hàng",
                    Icon = "❓",
                    BackgroundColor = "#6c757d",
                    MessageBackgroundColor = "#f8f9fa",
                    Message = "Trạng thái đơn hàng đang được cập nhật. Vui lòng liên hệ hotline để biết thêm chi tiết.",
                    AdditionalInfo = ""
                }
            };
        }

        public async Task<Order> UpdateStatus(UpdateOrderDto updateOrderDto)
        {
            /*
               0. chờ xác nhận
               1. đã xác nhận
               2. đang giao hàng
               3. giao hàng thành công
               4. đơn hàng thành công
               5. giao hàng không thành công
               6. đang hoàn hàng
               7. hoàn hàng thành công
               8. đã Hủy
            */
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var order = await _unitOfWork.Orders.GetByIdAsync(updateOrderDto.Id)
                ?? throw new Exception("Không tìm thấy đơn hàng");

                // log status
                LogUpdateSatus(order, updateOrderDto);

                switch (updateOrderDto.Status)
                {
                    case 0:
                        UpdateStatusTo0(order);
                        break;
                    case 1:
                        UpdateStatusTo1(order);
                        break;
                    case 2:
                        UpdateStatusTo2(order, updateOrderDto.TrackingCode);
                        break;
                    case 3:
                        UpdateStatusTo3(order);
                        break;
                    case 4:
                        UpdateStatusTo4(order);
                        break;
                    case 5:
                        UpdateStatusTo5(order);
                        break;
                    case 6:
                        UpdateStatusTo6(order);
                        break;
                    case 7:
                        UpdateStatusTo7(order);
                        break;
                    case 8:
                        await UpdateStatusTo8(order, updateOrderDto);
                        break;
                    default:
                        throw new Exception("Trạng thái không hợp lệ");
                }

               

                //await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return order;
            } 
            catch( Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.InnerException);
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            } 
            
        }

        private void LogUpdateSatus(Order order, UpdateOrderDto updateOrderDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            string userRole = user?.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new Exception("Lỗi trong quá trình lấy thông tin người dùng");
            string userName = user?.FindFirst(ClaimTypes.Name)?.Value
                ?? throw new Exception("Lỗi trong quá trình lấy thông tin người dùng");

            var currentTime = DateTime.Now;
            string currentStatusText = DisplayStatus(order.Status);
            var newStatusText = DisplayStatus(updateOrderDto.Status);
            var logEntry = 
                    $"\n---------Begin Log---------" +
                    $"\n[{currentTime:dd/MM/yyyy HH:mm:ss}] " +
                    $"\n{userName} - {userRole}" +
                    $"\nTrạng thái: {currentStatusText} → {newStatusText}";

            if (!string.IsNullOrEmpty(updateOrderDto.TrackingCode))
            {
                logEntry += $"\nMã vận đơn: {updateOrderDto.TrackingCode}";
            }

            // Thêm note từ user nếu có
            if (!string.IsNullOrEmpty(updateOrderDto.EmployeeNote))
            {
                logEntry += $"\nGhi chú: {updateOrderDto.EmployeeNote}";
            }
            logEntry += $"\n---------End Log---------\n";
            order.EmployeeNote += logEntry;
        }

        private void UpdateStatusTo0(Order order)
        {
            // chỉ có trạng thái 1 mới có thể chuyển về 0
            if (order.Status != 1 && order.Status != 0)
            {
                throw new Exception($"Không thể chuyển về trạng thái \"{DisplayStatus(order.Status)}\"");
            }
            order.Status = 0;

        }

        private void UpdateStatusTo1(Order order)
        {
            int[] priorStatuses = { 0, 1 };
            if(!priorStatuses.Contains(order.Status))
            {
                throw new Exception($"Không thể chuyển từ trạng thái \"{DisplayStatus(order.Status)}\" sang \"Đã xác nhận\"");
            }
            order.Status = 1;
        }
        private void UpdateStatusTo2(Order order, string? trackingCode)
        {
            int[] priorStatuses = { 1 };
            if(string.IsNullOrEmpty(trackingCode))
            {
                throw new Exception("Mã theo dõi không được để trống khi chuyển sang trạng thái 'Đang giao hàng'");
            }
            if (!priorStatuses.Contains(order.Status))
            {
                throw new Exception($"Không thể chuyển từ trạng thái \"{DisplayStatus(order.Status)}\" sang \"Đang giao hàng\"");
            }
            order.TrackingCode = trackingCode;
            order.Status = 2;
        }
        private void UpdateStatusTo3(Order order)
        {
            int[] priorStatuses = { 2 };
            if (!priorStatuses.Contains(order.Status))
            {
                throw new Exception($"Không thể chuyển từ trạng thái \"{DisplayStatus(order.Status)}\" sang \"Giao hàng thành công\"");
            }
            order.Status = 3;
        }
        private void UpdateStatusTo4(Order order)
        {
            int[] priorStatuses = { 2, 3 };
            if (!priorStatuses.Contains(order.Status))
            {
                throw new Exception($"Không thể chuyển từ trạng thái \"{DisplayStatus(order.Status)}\" sang \"Đơn hàng thành công\"");
            }
            order.Status = 4;
        }
        private void UpdateStatusTo5(Order order)
        {
            int[] priorStatuses = {2};
            if (!priorStatuses.Contains(order.Status))
            {
                throw new Exception($"Không thể chuyển từ trạng thái \"{DisplayStatus(order.Status)}\" sang \"Giao hàng không thành công\"");
            }
            order.Status = 5;
        }
        private void UpdateStatusTo6(Order order)
        {
            throw new Exception();
        }
        private void UpdateStatusTo7(Order order)
        {
            throw new Exception();
        }
        private async Task UpdateStatusTo8(Order order, UpdateOrderDto updateOrderDto)
        {
            // cộng lại số lượng
            await _unitOfWork.LoadCollectionAsync<Order, OrderDetail>(order, order => order.OrderDetails);
            var orderDetails = _unitOfWork.OrderDetails.GetAll()
                .Include(od => od.ProductDetail)
                .Where(od => od.OrderId == order.Id);
            foreach(var item in orderDetails)
            {
                item.ProductDetail.Quantity += item.Quantity;
            }

            int[] priorStatuses = { 0, 1 };
            if (!priorStatuses.Contains(order.Status))
            {
                throw new Exception($"Không thể hủy đơn hàng");
            }
            order.Status = 8;
        }

        private string DisplayStatus(int status)
        {
            return status switch
            {
                0 => "Chờ xác nhận",
                1 => "Đã xác nhận",
                2 => "Đang giao hàng",
                3 => "Giao hàng thành công",
                4 => "Đơn hàng thành công",
                5 => "Giao hàng không thành công",
                6 => "Đang hoàn hàng",
                7 => "Hoàn hàng thành công",
                8 => "Đã hủy",
                _ => throw new Exception("Trạng thái không hợp lệ")
            };
        }
    }
}
