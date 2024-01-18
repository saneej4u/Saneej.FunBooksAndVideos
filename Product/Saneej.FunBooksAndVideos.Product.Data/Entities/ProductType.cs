namespace Saneej.FunBooksAndVideos.Product.Data.Entities
{
    // Product types are Book, Video, Book Membership, Video Membership, Both Video & Book
    public class ProductType : EntityBase
    {
        public int ProductTypeId { get; set; }
        public string Code { get; set; } // BOOK, VIDEO, BOOK_MEMBERSHIP, VIDEO_MEMBERSHIP, PREMIUM_MEMBERSHIP
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
