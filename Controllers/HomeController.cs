using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DOTNET_Belt.Models;
using Microsoft.AspNetCore.Identity;

namespace DOTNET_Belt.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    [SessionCheck]
    public IActionResult Index()=>View("Index");

    [HttpPost("users/create")]   
    public IActionResult Register(User newUser){
        if(ModelState.IsValid){
            // Initializing a PasswordHasher object, providing our User class as its type            
            PasswordHasher<User> Hasher = new PasswordHasher<User>();   
            // Updating our newUser's password to a hashed version         
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);            
            //Save your user object to the database 
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            return RedirectToAction("Index", "Quest");
        } else {
            // handle else
            return Index();
        }   
    }

    [HttpPost("users/login")]
    public IActionResult Login(LoginUser userSubmission){
        Console.WriteLine(userSubmission.LoginEmail);
        if(ModelState.IsValid){        
            // If initial ModelState is valid, query for a user with the provided email        
            User? userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);        
            // If no user exists with the provided email        
            if(userInDb == null) {            
                // Add an error to ModelState and return to View!            
                ModelState.AddModelError("LoginEmail", "Invalid Email/Password");            
                return Index();
            }   
            // Otherwise, we have a user, now we need to check their password                 
            // Initialize hasher object        
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();                    
            // Verify provided password against hash stored in db        
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);                                    // Result can be compared to 0 for failure        
            if(result == 0)        
            {            
                // Handle failure (this should be similar to how "existing email" is handled)
                ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
                return Index();
            } 
            // Handle success (this should route to an internal page)
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return RedirectToAction("Index", "Quest");
        } else {
            // Handle else
            return Index();
        }
    }

    // Name this anything you want with the word "Attribute" at the end
    public class SessionCheckAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext context) {
            // Find the session, but remember it may be null so we need int?
            int? userId = context.HttpContext.Session.GetInt32("UserId");
            // Check to see if we got back null
            if(userId != null) {
                // Redirect to the Index page if there was nothing in session
                // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
                context.Result = new RedirectToActionResult("Index", "Quest", null);
            }
        }
    }

    public IActionResult Privacy()=>View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
