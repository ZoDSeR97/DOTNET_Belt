using System.ComponentModel.DataAnnotations;
using DOTNET_Belt.Models;

namespace DOTNET_Belt.Validations;
public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
    	// Though we have Required as a validation, sometimes we make it here anyways
    	// In which case we must first verify the value is not null before we proceed
        if(value == null)
            return new ValidationResult("Email is required!");// If it was, return the required error
        
    	// This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
        if(_context.Users.Any(e => e.Email == value.ToString()))
    	    // If yes, throw an error
            return new ValidationResult("Email must be unique!");
        return ValidationResult.Success;
    }
}

