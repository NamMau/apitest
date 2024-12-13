using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webshop.Models;

public class OrderDetail
{
    [Key]
    public int OrderDetailId { get; set; } // Ensure you have a primary key defined

    [ForeignKey("OrderId")]
    public int OrderId { get; set; }

    [ForeignKey("ProductId")]
    public int ProductId { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
}
