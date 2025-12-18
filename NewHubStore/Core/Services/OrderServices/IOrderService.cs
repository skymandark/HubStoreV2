using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.OrderViewModels;

namespace Core.Services.OrderServices
{
    public interface IOrderService
    {
        Task<OrderViewModel> CreateOrder(CreateOrderViewModel orderDto);
        Task<OrderViewModel> UpdateOrder(int orderId, UpdateOrderViewModel orderDto);
        Task<OrderViewModel> GetOrder(int orderId);
        Task<IEnumerable<OrderViewModel>> GetOrders(OrderFilterViewModel filters, PaginationViewModel pagination);
        Task<bool> DeleteOrder(int orderId);
        Task<bool> SubmitOrder(int orderId);
        Task<bool> CloseOrderManually(int orderId, string reason);
        Task<IEnumerable<OrderViewModel>> GetOrdersByBarcode(string barcode);
    }
}
