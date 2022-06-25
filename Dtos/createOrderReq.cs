using System.ComponentModel.DataAnnotations;

namespace StackApi.Dtos;

public class createOrderReq
{
    [Required(ErrorMessage = "Product is Required")]
    [Display(Name = "Product")]
    public Guid prdId { get; set; }
    public Guid? DId { get; set; }
    [Required(ErrorMessage = "Order Quantity is Required")]
    [Range(1, 5, ErrorMessage = "Invalid Quantity")]
    public int Qty { get; set; }
    [Required(ErrorMessage = "Cart is Required")]
    [Display(Name = "Cart")]
    public Guid Cid { get; set; }
}