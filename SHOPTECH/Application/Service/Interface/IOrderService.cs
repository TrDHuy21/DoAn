using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.OrderDtos;
using Domain.Enity;

namespace Application.Service.Interface
{
    public interface IOrderService
    {
        Task<Order> CreateOnlineOrder(CreateOnlineOrderDto createOrderOnlineDto); 
        Task<Order> CreateOfflineOrder(CreateOfflineOrderDto createOfflineOrderDto);
        Task<IEnumerable<Order>> GetAll(int? status);
        Task<IEnumerable<Order>> GetAllByCustomerId(int userId);
        Task<IEnumerable<Order>> GetAllByEmployeeId(int userId);
        Task<IEnumerable<Order>> GetMyOrder();
        Task<Order> GetById(int id);

        Task<Order> UpdateStatus(UpdateOrderDto updateOrderDto);

        Task SendNewOrderNotificationForAdminAsync(Order order);
        Task SendNewOrderNotificationForCustomerAsync(Order order);
        Task SendOrderStatusForCustomerAsync(Order order);
    }
}
