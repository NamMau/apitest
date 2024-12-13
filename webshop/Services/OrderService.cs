using AutoMapper;
using webshop.Data;
using webshop.Dtos;
using webshop.Models;

namespace webshop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }
            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.ShippingProviderName = order.ShippingProvider.Name;
            return orderDto;
        }

        public async Task CreateOrderAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteOrderDetailAsync(int orderId, int productId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order != null)
            {
                var orderDetail = order.OrderDetails.FirstOrDefault(od => od.ProductId == productId);
                if (orderDetail != null)
                {
                    order.OrderDetails.Remove(orderDetail);
                    _unitOfWork.Orders.Update(order);
                    await _unitOfWork.CompleteAsync();
                }
            }
        }

        public async Task<IEnumerable<OrderDto>> SearchOrdersByProductNameAsync(string keyword)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var filteredOrders = orders.Where(o => o.OrderDetails.Any(od => od.Product.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
            return _mapper.Map<IEnumerable<OrderDto>>(filteredOrders);
        }
    }
}
