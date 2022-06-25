namespace StackApi.Models;

public class Part
{
    public Guid Pid { get; set; }
    public string PartName { get; set; }
    public string PartDesc { get; set; }
    public decimal PartPrice { get; set; }
    public DateTime ModifiedOn { get; set; }
    public ICollection<PartImages> PartImages { get; set; }
    public Guid PcId { get; set; }
    public Category category { get; set; }
    public ICollection<Discount> Discounts { get; set; }
    public ICollection<CartItems> cartItems { get; set; }
    public ICollection<OrderItems> OrderItems { get; set; }
}