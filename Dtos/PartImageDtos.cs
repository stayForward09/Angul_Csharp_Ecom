using System.ComponentModel.DataAnnotations;
using AutoMapper;
using StackApi.Validators;

namespace StackApi.Dtos;

public class PartImageDtos
{
    [Required(ErrorMessage = "Atleast one file is required")]
    [Display(Name = "Part Image")]
    [AllowedExtensionsAttribute(new string[] { ".jpg", ".jpeg", ".png", ".glb",".gif" })]
    public List<IFormFile> PartFiles { get; set; }

    [Display(Name = "Part #")]
    [Required(ErrorMessage = "Part ID is Required")]
    public Guid PartID { get; set; }
}