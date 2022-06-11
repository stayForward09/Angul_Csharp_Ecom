namespace StackApi.Models;

public class CartItems
{
    public Guid CITId { get; set; }
    public Guid CIPrid { get; set; }
    public int CIQty { get; set; }
    public Guid CIUsid { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public User User { get; set; }
    public Part Part { get; set; }
}