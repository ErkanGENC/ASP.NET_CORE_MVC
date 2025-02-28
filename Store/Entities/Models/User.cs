using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
} 