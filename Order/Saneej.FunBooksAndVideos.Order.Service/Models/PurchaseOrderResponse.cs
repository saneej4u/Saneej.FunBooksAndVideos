namespace Saneej.FunBooksAndVideos.Service.Models
{
    public class PurchaseOrderResponse
    {
        public int PurchaseOrderId { get; set; }
        public string Status { get; set; } // Paid, Not paid, Cancelled etc.
        public decimal Total { get; set; }
        public int CustomerId { get; set; }
        public bool IsMembershipActivated { get; set; }
        public bool IsShippingSlipCreated { get; internal set; }
        public List<PurchaseOrderLineResponse> PurchaseOrderLines { get; set; }
    }
}
