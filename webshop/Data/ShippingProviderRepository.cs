using Microsoft.EntityFrameworkCore;
using webshop.Models;

namespace webshop.Data
{
    public class ShippingProviderRepository : IRepository<ShippingProvider>
    {
        private readonly AppDbContext _context;

        public ShippingProviderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ShippingProvider> GetByIdAsync(int id)
        {
            return await _context.ShippingProviders.FindAsync(id);
        }

        public async Task<IEnumerable<ShippingProvider>> GetAllAsync()
        {
            return await _context.ShippingProviders.ToListAsync();
        }

        public async Task AddAsync(ShippingProvider entity)
        {
            await _context.ShippingProviders.AddAsync(entity);
        }

        public void Update(ShippingProvider entity)
        {
            _context.ShippingProviders.Update(entity);
        }

        public void Delete(ShippingProvider entity)
        {
            _context.ShippingProviders.Remove(entity);
        }
    }
}
