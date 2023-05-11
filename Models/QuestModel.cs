using System.ComponentModel.DataAnnotations;
namespace DOTNET_Belt.Models;

public class Quest{
    [Key]
    public int QuestId { get;set; }
    [Required]
    public string? Name { get;set; }
    [Required]
    public string? Description {get; set;}
    [Required]
    public int Gold { get; set; }
    public int UserId { get; set; }

    public bool Closed {get; set;} = false;

    //Navigation Properties
    public User? Creator {get; set;}
    public List<Progress> Players {get; set;} = new List<Progress>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}