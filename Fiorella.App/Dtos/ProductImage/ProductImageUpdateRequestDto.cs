namespace Fiorella.App.Dtos.ProductImage
{
    public record ProductImageUpdateRequestDto
    {
        public int ClickedImageId { get; set; }
        public int ProductId { get; set; }
    }
}
