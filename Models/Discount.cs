namespace StackApi.Models;

public class Discount
{
    public Guid Did { get; set; }
    public string CouponCode { get; set; }
    public string CouponName { get; set; }
    public decimal Amount { get; set; }
    public int DType { get; set; }
    public Guid? PrdId { get; set; }
    public Guid? CId { get; set; }
    public bool Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Part Parts { get; set; }
    public Category Categories { get; set; }
}