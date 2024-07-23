namespace Fiorella.App.Dtos.ProductImage
{
    public record ProductImagePostDto
    {
        public string Url { get; set; } = string.Empty;
        public bool IsMain { get; set; }

    }
}
