using System.ComponentModel.DataAnnotations;

namespace DOTNET_Belt.Validations;
public class FutureDateAttribute : ValidationAttribute
{    
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)    
    {        
        // You first may want to unbox "value" here and cast to to a DateTime variable!
        if(value != null){
            if(DateTime.Compare((DateTime)value, DateTime.Today)>0)
                return ValidationResult.Success;
            if(DateTime.Compare((DateTime)value, DateTime.Today)==0)
                return new ValidationResult("We don't plan shotgun wedding!");
        }
        return new ValidationResult("Date is already past...");
    }
}
