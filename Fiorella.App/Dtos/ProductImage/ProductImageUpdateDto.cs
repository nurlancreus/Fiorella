namespace Fiorella.App.Dtos.ProductImage
{
    public record ProductImageUpdateDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool IsMain { get; set; }

    }
}
