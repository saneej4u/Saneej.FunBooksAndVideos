using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.Mappers
{
    public interface IPurchaseOrderMapper
    {
        PurchaseOrderResponse MapToPurchaseOrderResponseFromEntity(Data.Entities.PurchaseOrder purchaseOrderEntity);
    }
}
