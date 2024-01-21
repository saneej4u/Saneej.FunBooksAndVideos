namespace Saneej.FunBooksAndVideos.Service.Models
{
    //public record ProductViewModel(
    //    int ProductId,
    //    string ProductCode,
    //    string Name,
    //    string Description,
    //    string ProductTypeCode,
    //    decimal Price,
    //    int Stock,
    //    bool IsPhysicalProduct);

    // BOOK, VIDEO, BOOK_MEMBERSHIP, VIDEO_MEMBERSHIP, PREMIUM_MEMBERSHIP
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductTypeCode { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsPhysicalProduct { get; set; }
    };
}
