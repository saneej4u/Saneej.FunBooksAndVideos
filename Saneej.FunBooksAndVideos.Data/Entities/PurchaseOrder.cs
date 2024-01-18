namespace Saneej.FunBooksAndVideos.Data.Entities
{
    public class PurchaseOrder : EntityBase
    {
        public PurchaseOrder()
        {
            PurchaseOrderLines = new HashSet<PurchaseOrderLine>();
        }
        public int PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; } // Paid, Not paid, Cancelled etc.
        public decimal Total { get; set; }
        public ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }
    }
}
