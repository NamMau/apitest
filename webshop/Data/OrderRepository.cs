using Microsoft.EntityFrameworkCore;
using webshop.Models;

namespace webshop.Data
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.OrderDetails)
                                        .Include(o => o.ShippingProvider)
                                        .FirstOrDefaultAsync(o => o.OrderID == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.OrderDetails)
                                        .Include(o => o.ShippingProvider)
                                        .ToListAsync();
        }

        public async Task AddAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);
        }

        public void Update(Order entity)
        {
            _context.Orders.Update(entity);
        }

        public void Delete(Order entity)
        {
            _context.Orders.Remove(entity);
        }
    }
}
