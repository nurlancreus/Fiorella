namespace Fiorella.App.Dtos.Tag
{
    public record TagGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
