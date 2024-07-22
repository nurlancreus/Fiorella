namespace Fiorella.App.Dtos.ProductImage
{
    public record ProductImageGetDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }
}
