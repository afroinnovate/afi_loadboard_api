using System.ComponentModel.DataAnnotations;

namespace Auth.Min.API.Dtos
{
    public class RegistrationModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
        public required string Password { get; set; }
        public string? UserType { get; set; }
    }
}