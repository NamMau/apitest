using System.Threading.Tasks;
using webshop.Models;

namespace webshop.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Orders = new OrderRepository(context);
            Products = new ProductRepository(context);
            ShippingProviders = new ShippingProviderRepository(context);
            Customers = new CustomerRepository(context);
        }

        public IRepository<Order> Orders { get; }
        public IRepository<Product> Products { get; }
        public IRepository<ShippingProvider> ShippingProviders { get; }
        public IRepository<Customer> Customers { get; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
