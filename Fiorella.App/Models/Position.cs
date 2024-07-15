using Fiorella.App.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Fiorella.App.Models
{
    public class Position : BaseEntity
    {

        public Position()
        {

            Employees = new HashSet<Employee>();
        }

        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<Employee> Employees { get; set; }
    }
}
