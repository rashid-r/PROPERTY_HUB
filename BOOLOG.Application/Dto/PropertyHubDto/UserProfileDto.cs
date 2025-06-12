using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOGAM.Domain.Model;
using System.ComponentModel.DataAnnotations;
using BOOLOG.Domain.Model;

public class  UserProfileDto
{
    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(UserProfileDto), nameof(ValidateDateOfBirth))]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other.")]
    public Genders Gender { get; set; }

    [Required(ErrorMessage = "Aadhaar ID is required.")]
    [Range(100000000000, 999999999999, ErrorMessage = "Aadhaar ID must be a 12-digit number.")]
    public long AadhaarIdNumber { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "First character must be a letter. Only letters and spaces are allowed.")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "First character must be a letter. Only letters and spaces are allowed.")]
    [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.")]
    public string City { get; set; }

    [Required(ErrorMessage = "State is required.")]
    [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "First character must be a letter. Only letters and spaces are allowed.")]
    [StringLength(100, ErrorMessage = "State name cannot exceed 100 characters.")]
    public string State { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "First character must be a letter. Only letters and spaces are allowed.")]
    [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters.")]
    public string Country { get; set; }

    [Required(ErrorMessage = "Postal code is required.")]
    public double PostalCode { get; set; }




    
    public static ValidationResult ValidateDateOfBirth(DateTime date, ValidationContext context)
    {
        if (date > DateTime.Today)
        {
            return new ValidationResult("Date of birth cannot be in the future.");
        }
        return ValidationResult.Success;
    }
}

