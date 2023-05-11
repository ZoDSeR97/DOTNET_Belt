using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using DOTNET_Belt.Models;

namespace DOTNET_Belt.Validations;
public class UsernameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
    	// Though we have Required as a validation, sometimes we make it here anyways
    	// In which case we must first verify the value is not null before we proceed
        if(value == null)
            return new ValidationResult("Username is required!");// If it was, return the required error
        Regex rx = new Regex(@"^[a-zA-Z]+$");
    	// This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
        if(_context.Users.Any(e => e.UserName == value.ToString()))
    	    // If yes, throw an error
            return new ValidationResult("Username is already in used");
        if(!rx.IsMatch(value.ToString()))
            return new ValidationResult("Only Letter is allow for Username");
        return ValidationResult.Success;
    }
}