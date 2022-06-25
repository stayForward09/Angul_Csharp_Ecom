namespace StackApi.Models;

public class OrdersDiscount
{
    public Guid ODid { get; set; }
    public Guid OIid { get; set; }
    public string CouponCode { get; set; }
    public string CouponName { get; set; }
    public decimal Amount { get; set; }
    public int DType { get; set; }
    public OrderItems OrderItems {get;set;}
}