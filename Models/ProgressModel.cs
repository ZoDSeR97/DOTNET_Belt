using System.ComponentModel.DataAnnotations;
namespace DOTNET_Belt.Models;

public class Progress{
    [Key]
    public int ProgressId { get;set; }
    public int UserId { get;set; }
    public int QuestId {get; set;}
    public bool Completed {get; set;} = false;

    public User? Player {get; set;}
    public Quest? Quest {get; set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}