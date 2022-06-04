using System;
using System.ComponentModel.DataAnnotations;

namespace StackApi.Models;

public class User
{
    [Key]
    public Guid UsID { get; set; }
    public string Fname { get; set; } = null!;
    public string Mname { get; set; } = null!;
    public string Lname { get; set; } = null!;
    public DateTime DOB { get; set; }
    public string EmailID { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
}