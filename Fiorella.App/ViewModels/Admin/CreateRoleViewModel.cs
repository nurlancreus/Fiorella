using System.ComponentModel.DataAnnotations;

namespace Fiorella.App.ViewModels.Admin
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
        public string? Description { get; set; }
    }
}
