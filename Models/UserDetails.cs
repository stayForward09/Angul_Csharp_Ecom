// UserDetails

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackApi.Models;

public class UserDetails
{
    public Guid UsId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyCategory { get; set; }
}