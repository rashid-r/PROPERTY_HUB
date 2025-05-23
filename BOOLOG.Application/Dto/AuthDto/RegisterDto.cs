using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class RegisterDto
{
    [Required(ErrorMessage = "Contact (Email or Phone Number) is required.")]
    [StringLength(50, ErrorMessage = "Contact must be at most 50 characters.")]
    [ContactValidation]
    public string Contact { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 100 characters.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}


public class ContactValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("Contact is required.");

        string contact = value.ToString().Trim();

        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        string phonePattern = @"^\+?[0-9]{10}$";

        bool isValidEmail = Regex.IsMatch(contact, emailPattern);
        bool isValidPhone = Regex.IsMatch(contact, phonePattern);

        if (isValidEmail || isValidPhone)
            return ValidationResult.Success;

        return new ValidationResult("Please enter a valid email address or phone number.");
    }
}
