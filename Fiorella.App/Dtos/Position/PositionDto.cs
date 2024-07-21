using Fiorella.App.Dtos.Employee;

namespace Fiorella.App.Dtos.Position
{
    public record PositionDto
    {
        public PositionDto()
        {
            Employees = new HashSet<EmployeeGetDto>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<EmployeeGetDto> Employees { get; set; }

    }
}
