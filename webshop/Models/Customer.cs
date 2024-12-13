namespace webshop.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; } = string.Empty; // Set default value
        public string Address { get; set; } = string.Empty; // Set default value
        public string Phone { get; set; } = string.Empty; // Set default value
        public string Email { get; set; } = string.Empty; // Set default value
        public string Password { get; set; } = string.Empty; // Set default value
        public bool IsVIP { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalOrders { get; set; } // Add this line

    }
}
