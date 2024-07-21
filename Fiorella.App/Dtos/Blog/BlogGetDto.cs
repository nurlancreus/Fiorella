namespace Fiorella.App.Dtos.Blog
{
    public record BlogGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = "default-img.jpg";
        public DateTime CreatedAt { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
