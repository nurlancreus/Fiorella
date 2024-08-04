using System.ComponentModel.DataAnnotations;

namespace Fiorella.App.ViewModels.Admin
{
    public class UpdateRoleViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Role Name is Required")]
        public string RoleName { get; set; }
        public string? Description { get; set; }
        public List<string> Users { get; set; } = [];
    }
}
