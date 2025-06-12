using System.ComponentModel.DataAnnotations;

namespace BOOLOG.Application.Dto.AuthDto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email Or MobileNumber is required.")]
        [StringLength(50)]
        [ContactValidation]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

