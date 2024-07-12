using Fiorella.App.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiorella.App.Models
{
    public class Blog : BaseEntity
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? FormFile { get; set; }
    }
}
