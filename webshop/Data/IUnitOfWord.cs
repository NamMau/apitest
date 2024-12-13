using webshop.Models;

namespace webshop.Data
{
    public interface IUnitOfWork
    {
        IRepository<Order> Orders { get; }
        IRepository<Product> Products { get; }
        IRepository<ShippingProvider> ShippingProviders { get; }
        IRepository<Customer> Customers { get; }
        Task<int> CompleteAsync();
    }
}
