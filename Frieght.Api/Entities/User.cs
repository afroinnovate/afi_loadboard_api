using Frieght.Api.Enuns;

namespace Frieght.Api.Entities
{
    public class User
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public string? MiddleName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Phone { get; set; }
        public required string UserType { get; set; } // Shipper or Carrier

        // Navigation properties
        public virtual ICollection<Load>? Loads { get; set; } = new List<Load>();
        public virtual ICollection<Bid>? Bids { get; set; } = new List<Bid>();
        public virtual BusinessProfile? BusinessProfile { get; set; }
    }
}
