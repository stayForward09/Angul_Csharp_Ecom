using System.ComponentModel.DataAnnotations;
namespace StackApi.Dtos;

public class VerifyOTP
{
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email ID")]
    [Required(ErrorMessage = "Email ID Is Required")]
    [Display(Name = "Email ID")]
    public string EmailID { get; set; }

    [MaxLength(6, ErrorMessage = "Invalid OTP")]
    [Required(ErrorMessage = "OTP Is Required")]
    [Display(Name = "OTP")]
    public string OTP { get; set; }
}