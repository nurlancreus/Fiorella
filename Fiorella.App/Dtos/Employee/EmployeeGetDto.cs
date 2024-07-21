using Fiorella.App.Dtos.Position;

namespace Fiorella.App.Dtos.Employee
{
    public record EmployeeGetDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Image { get; set; } = "default-avatar.jpg";
        public PositionDto? Position { get; set; }
        public IFormFile? FormFile { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
