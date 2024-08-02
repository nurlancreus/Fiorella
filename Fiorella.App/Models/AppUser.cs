using Microsoft.AspNetCore.Identity;

namespace Fiorella.App.Models
{
    public class AppUser : IdentityUser<int>
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }
    }
}
