using Microsoft.AspNetCore.Identity;

namespace Fiorella.App.Models
{
    public class AppRole : IdentityRole<int>
    {
        public string? Description { get; set; }
    }
}
