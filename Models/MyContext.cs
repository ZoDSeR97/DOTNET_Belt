using Microsoft.EntityFrameworkCore;

namespace DOTNET_Belt.Models;
// the MyContext class represents a session with our MySQL database, allowing us to query for or save data
// DbContext is a class that comes from EntityFramework, we want to inherit its features
public class MyContext : DbContext 
{   
    // This line will always be here. It is what constructs our context upon initialization
    public MyContext(DbContextOptions options) : base(options) { }
    // We need to create a new DbSet<Model> for every model in our project that is making a table
    // The name of our table in our database will be based on the name we provide here
    // This is where we provide a plural version of our model to fit table naming standards    
    public DbSet<User> Users { get; set; } 
    public DbSet<Progress> Progresses { get; set; }
    public DbSet<Quest> Quests { get; set; }
}