using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DOTNET_Belt.Validations;

namespace DOTNET_Belt.Models;

public class User{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MinLength(2)]
    [Username]
    public string? UserName {get; set;}

    [Required]
    [EmailAddress]
    [UniqueEmail]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string? Password { get; set; }

    public int Gold {get; set;} = 0;

    [Required]
    [NotMapped]
    [Compare("Password")]
    public string? PasswordConfirm { get; set; }

    public List<Quest> PostedQuests {get; set;} = new List<Quest>();
    public List<Progress> TakenQuests {get; set;} = new List<Progress>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}