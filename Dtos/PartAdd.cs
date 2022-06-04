using System.ComponentModel.DataAnnotations;

namespace StackApi.Dtos;

public class PartAdd
{
    [Required(ErrorMessage = "Part Name is Required")]
    [Display(Name = "Part Name")]
    [MinLength(5, ErrorMessage = "Minimun Length of 5 ")]
    [MaxLength(100, ErrorMessage = "Maximum Length of 100")]
    public string PartName { get; set; }

    [Required(ErrorMessage = "Part Description is Required")]
    [Display(Name = "Part Description")]
    public string PartDesc { get; set; }

    [Required(ErrorMessage = "Part Price is Required")]
    [Display(Name = "Part Price")]
    [DataType(DataType.Currency)]
    public decimal PartPrice { get; set; }
    [Required(ErrorMessage = "Part Category is Required")]
    [Display(Name = "Part Category")]
    public Guid PartCategory { get; set; }
}