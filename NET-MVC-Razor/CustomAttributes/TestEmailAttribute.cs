using NET_MVC_Razor.Data;
using System.ComponentModel.DataAnnotations;

namespace NET_MVC_Razor.CustomAttributes
{
    public class TestEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
        ValidationContext validationContext)
        {
            var context = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            if (!context.Employee.Any(a => a.Email == value.ToString()))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Email already exists");
        }
    }
}
