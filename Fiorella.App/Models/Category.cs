using Fiorella.App.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Fiorella.App.Models
{
    public class Category : BaseEntity
    {
        [Required(ErrorMessage = "The name is required.")]
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
    }
}
