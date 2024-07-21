using Fiorella.App.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiorella.App.Models
{
    public class Employee : BaseEntity
    {
        //[Required]
        public string FirstName { get; set; } = string.Empty;
        //[Required]
        public string LastName { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
        public int? PositionId { get; set; }
        public Position? Position { get; set; }
        //[NotMapped]
        //public IFormFile? FormFile { get; set; }
    }
}
