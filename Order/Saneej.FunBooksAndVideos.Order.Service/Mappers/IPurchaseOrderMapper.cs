using Saneej.FunBooksAndVideos.Data.Entities;
using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.Mappers
{
    public interface IPurchaseOrderMapper
    {
        PurchaseOrderResponse MapToPurchaseOrderResponse(Data.Entities.PurchaseOrder purchaseOrderEntity);
        PurchaseOrderLine MapToPurchaseOrderLine(ProductViewModel productItem, int quantity);
        Data.Entities.PurchaseOrder MapToPurchaseOrder(List<PurchaseOrderLine> orderLines, string orderStatus, decimal orderTotal, int customerId);
    }
}
