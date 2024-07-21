using Fiorella.App.Dtos.Position;

namespace Fiorella.App.Dtos.Employee
{
    public record EmployeeUpdateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Image { get; set; } = "default-avatar.jpg";
        public int? PositionId { get; set; }
        public PositionDto? Position { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
