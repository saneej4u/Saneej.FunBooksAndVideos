namespace Saneej.FunBooksAndVideos.Service.Models
{
    public record ProductViewModel(
        int ProductId,
        string ProductCode,
        string Name,
        string Description,
        string ProductTypeCode,
        decimal Price,
        int Stock,
        bool IsPhysicalProduct);

    // BOOK, VIDEO, BOOK_MEMBERSHIP, VIDEO_MEMBERSHIP, PREMIUM_MEMBERSHIP
    //public record class ProductTypeViewModel(
    //    int ProductTypeId,
    //    string Code,
    //    string Name,
    //    string Description);
}
