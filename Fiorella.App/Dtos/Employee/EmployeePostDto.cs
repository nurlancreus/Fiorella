using Fiorella.App.Dtos.Position;

namespace Fiorella.App.Dtos.Employee
{
    public record EmployeePostDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Image { get; set; }
        public int? PositionId { get; set; }
        public PositionDto? Position { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
