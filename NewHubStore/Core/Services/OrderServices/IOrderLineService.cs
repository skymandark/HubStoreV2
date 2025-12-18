using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.OrderViewModels;

namespace Core.Services.OrderServices
{
    public interface IOrderLineService
    {
        Task<OrderLineViewModel> AddOrderLine(int orderId, CreateOrderLineViewModel lineDto);
        Task<OrderLineViewModel> UpdateOrderLine(int lineId, UpdateOrderLineViewModel lineDto);
        Task<bool> DeleteOrderLine(int lineId);
        Task<IEnumerable<OrderLineViewModel>> GetOrderLines(int orderId);
        Task<bool> RecordPartialReceive(int lineId, decimal qtyReceived, int movementId);
    }
}
