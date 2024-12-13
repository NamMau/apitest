using Microsoft.AspNetCore.Mvc;
using webshop.Dtos;
using webshop.Services;

namespace webshop.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            _logger.LogInformation("Getting all orders");
            var orders = await _orderService.GetOrdersAsync();
            _logger.LogInformation("Retrieved {OrderCount} orders", orders.Count());
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            _logger.LogInformation("Getting order with ID {OrderId}", orderId);
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found", orderId);
                return NotFound();
            }
            _logger.LogInformation("Retrieved order with ID {OrderId}", orderId);
            return Ok(order);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchOrders(string keyword)
        {
            _logger.LogInformation("Searching orders with keyword {Keyword}", keyword);
            var orders = await _orderService.SearchOrdersByProductNameAsync(keyword);
            _logger.LogInformation("Found {OrderCount} orders with keyword {Keyword}", orders.Count(), keyword);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            _logger.LogInformation("Creating a new order");
            await _orderService.CreateOrderAsync(orderDto);
            _logger.LogInformation("Created order with ID {OrderId}", orderDto.OrderID);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = orderDto.OrderID }, orderDto);
        }

        [HttpDelete("{orderId}/products/{productId}")]
        public async Task<IActionResult> DeleteOrderDetail(int orderId, int productId)
        {
            _logger.LogInformation("Deleting product {ProductId} from order {OrderId}", productId, orderId);
            await _orderService.DeleteOrderDetailAsync(orderId, productId);
            _logger.LogInformation("Deleted product {ProductId} from order {OrderId}", productId, orderId);
            return NoContent();
        }
    }
}
