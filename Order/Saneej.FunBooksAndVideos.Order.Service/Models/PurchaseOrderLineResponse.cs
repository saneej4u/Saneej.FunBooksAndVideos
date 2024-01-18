namespace Saneej.FunBooksAndVideos.Service.Models
{
    public class PurchaseOrderLineResponse
    {
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductTypeCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsPhysicalProduct { get; set; }
    }
}
