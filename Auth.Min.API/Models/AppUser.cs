using Microsoft.AspNetCore.Identity;

namespace Auth.Min.API.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime? DateLastLoggedIn { get; set; }
        public string? MiddleName { get; set; }
    }
}