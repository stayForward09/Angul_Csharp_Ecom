namespace StackApi.Models;

public class OrderItems
{
    public Guid OIid { get; set; }
    public Guid Oid { get; set; }
    public Guid Prid { get; set; }
    public decimal ListPrice { get; set; }
    public decimal OrderPrice { get; set; }
    public int Qty { get; set; }
    public Orders Orders { get; set; }
    public OrdersDiscount OrdersDiscount { get; set; }
    public Part Part {get;set;}
}