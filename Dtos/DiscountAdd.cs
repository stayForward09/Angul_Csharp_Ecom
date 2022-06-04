using System.ComponentModel.DataAnnotations;

namespace StackApi.Dtos;

public class DiscountAdd
{
    [Required(ErrorMessage = "Coupon Code is Required")]
    [Display(Name = "Coupon Code")]
    [StringLength(20, ErrorMessage = "Max Length Exceed")]
    public string CouponCode { get; set; }

    [Required(ErrorMessage = "Coupon Name is Required")]
    [Display(Name = "Coupon Name")]
    [StringLength(30, ErrorMessage = "Max Length Exceed")]
    public string CouponName { get; set; }

    [Required(ErrorMessage = "Amount is Required")]
    [Display(Name = "Amount")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Discount Type is Required")]
    [Display(Name = "Discount Type")]
    public int DType { get; set; }

    [Display(Name = "Product ID")]
    public Guid? PrdId { get; set; }

    [Display(Name = "Category ID")]
    public Guid? CId { get; set; }

    [Display(Name = "Status")]
    public bool Status { get; set; }

    [Required(ErrorMessage = "Start Date is Required")]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End Date is Required")]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }
}