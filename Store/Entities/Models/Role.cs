using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
} 