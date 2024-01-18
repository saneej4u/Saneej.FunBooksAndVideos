namespace Saneej.FunBooksAndVideos.Service.Models
{
    public class BasketRequest
    {
        public int CustomerId { get; set; }
        public List<BasketItemRequest> BasketItems { get; set; }
    }

    public class BasketItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
