
using System.ComponentModel.DataAnnotations;

public class CartItemsAdd
{
    public Guid CITId { get; set; }
    
    [Required(ErrorMessage = "Product is Required")]
    [Display(Name = "Product Name")]
    public Guid CIPrid { get; set; }

    [Required(ErrorMessage = "Quantity is Required")]
    [Display(Name = "Quantity")]
    [Range(1, 5, ErrorMessage = "Maximum allowed Quantity 5")]
    public int CIQty { get; set; }
}