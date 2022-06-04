namespace StackApi.Models;

public class Category
{
    public Guid CId { get; set; }
    public string CName { get; set; }
    public bool CisActive { get; set; }
    public ICollection<Part> Parts { get; set; }
    public ICollection<Discount> Discounts { get; set; }
}