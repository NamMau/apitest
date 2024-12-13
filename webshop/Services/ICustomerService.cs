using System.Collections.Generic;
using System.Threading.Tasks;
using webshop.Dtos;

namespace webshop.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> GetCustomerByIdAsync(int id);
        Task UpdateCustomerAsync(CustomerDto customerDto);
    }
}
