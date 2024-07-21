namespace Fiorella.App.Dtos.ProductImage
{
    public record ProductImageGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
