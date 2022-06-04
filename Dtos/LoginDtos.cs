using System.ComponentModel.DataAnnotations;
namespace StackApi.Dtos;

public class LoginDtos
{
    [Required(ErrorMessage = "Username is Required")]
    [Display(Name = "Username")]
    [DataType(DataType.EmailAddress)]
    public string emailID { get; set; }

    [Required(ErrorMessage = "Password is Required")]
    [Display(Name = "Password")]
    [StringLength(20)]
    public string Password { get; set; }
}