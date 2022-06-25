namespace StackApi.Models;

public class Orders
{
    public Guid Oid { get; set; }
    public string PayRef { get; set; }
    public int PayType { get; set; }
    /*
    0 - online payment 
    1 - COD
    */
    public int PayStatus { get; set; }
    /*
    0- link created
    1- paid
    2- not paid
    */
    public string Address { get; set; }
    public decimal TotalPrice { get; set; }
    public Guid UsId { get; set; }
    public DateTime CreatedDate { get; set; }
    public ICollection<OrderItems> OrderItems { get; set; }
    public User User { get; set; }
}