namespace Fiorella.App.Dtos.Blog
{
    public record BlogGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; } = "default-img.jpg";
        public IFormFile? FormFile { get; set; }
    }
}
