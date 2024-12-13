using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using webshop.Data;
using webshop.Models;

namespace webshop.Jobs
{
    public class VipStatusJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VipStatusJob> _logger;

        public VipStatusJob(IUnitOfWork unitOfWork, ILogger<VipStatusJob> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();
            foreach (var customer in customers)
            {
                if (ShouldBecomeVip(customer))
                {
                    customer.IsVIP= true;
                    _unitOfWork.Customers.Update(customer);
                }
            }
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("VipStatusJob executed successfully");
        }

        private bool ShouldBecomeVip(Customer customer)
        {
            // Custom logic to determine if a customer should become VIP
            return customer.TotalOrders > 50; 
        }
    }
}
