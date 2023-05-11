using System.ComponentModel.DataAnnotations;
namespace DOTNET_Belt.Models;
public class LoginUser{
    // No other fields!
    [Required]
    [Display(Name="Email")] 
    public string? LoginEmail { get; set; }
    [Required]
    [Display(Name="Password")]
    public string? LoginPassword { get; set; }
}
