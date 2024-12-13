namespace webshop.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Decimal? Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreateAt { get; set; }
        public int ShopID { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
