namespace Saneej.FunBooksAndVideos.Data.Entities
{
    public class PurchaseOrderLine : EntityBase
    {
        public int PurchaseOrderLineId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductTypeCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsPhysicalProduct { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
