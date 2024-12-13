using System;
using System.Collections.Generic;

namespace webshop.Dtos
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime OverDueDate { get; set; }
        public DateTime? PaidAt { get; set; }
        public string DeliveryStatus { get; set; }
        public string ShippingProviderName { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
