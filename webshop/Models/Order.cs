namespace webshop.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime OverDueDate { get; set; }
        public DateTime? PaidAt { get; set; }
        public string DeliveryStatus { get; set; }
        public int ShippingProviderID { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public ShippingProvider ShippingProvider { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
