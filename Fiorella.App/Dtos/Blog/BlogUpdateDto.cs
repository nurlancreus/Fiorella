namespace Fiorella.App.Dtos.Blog
{
    public record BlogUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public IFormFile? FormFile { get; set; }
    }
}
