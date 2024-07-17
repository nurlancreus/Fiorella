namespace Fiorella.App.Dtos.Blog
{
    public record BlogUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
