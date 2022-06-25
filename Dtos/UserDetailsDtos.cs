using System.ComponentModel.DataAnnotations;

namespace StackApi.Dtos;

public class UserDetailsDtos
{
    public string CompanyName { get; set; }
    public string CompanyCategory { get; set; }
    
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }
}