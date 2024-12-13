using System.Collections.Generic;
using System.Threading.Tasks;
using webshop.Dtos;

namespace webshop.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task CreateOrderAsync(OrderDto orderDto);
        Task DeleteOrderDetailAsync(int orderId, int productId);
        Task<IEnumerable<OrderDto>> SearchOrdersByProductNameAsync(string keyword);
    }
}
