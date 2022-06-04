using System.ComponentModel.DataAnnotations;

namespace StackApi.Validators;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] filextension;
    public AllowedExtensionsAttribute(string[] allowedExtensions)
    {
        filextension = allowedExtensions;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var files = value as List<IFormFile>;
        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!filextension.Any(x => x == extension.ToLower()))
            {
                return new ValidationResult("Inavlid File Format");
            }
        }
        return ValidationResult.Success;
    }
}