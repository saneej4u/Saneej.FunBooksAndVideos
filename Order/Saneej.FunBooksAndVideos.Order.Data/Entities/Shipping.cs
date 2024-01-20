namespace Saneej.FunBooksAndVideos.Data.Entities
{
    public class Shipping : EntityBase
    {
        public int ShippingId { get; set; }
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public int OrderLineId { get; set; }
        public string ShippingStatus { get; set; }
    }
}
