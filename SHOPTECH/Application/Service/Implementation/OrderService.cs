using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.OrderDtos;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<Order> CreateOfflineOrder(CreateOnlineOrderDto createOrderOnlineDto)
        {
            throw new NotImplementedException();

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
            .FirstOrDefaultAsync(x => x.Id == id && (x.CustomerId == userId ||role == "admin" || role == "saler"))
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
            else if (role == "Employee")
            {
                return await GetAllByEmployeeId(userId);
            }
            else
            {
                throw new Exception("Lỗi trong quá trình lấy thông tin người dùng");
            }
        }
    }
}
