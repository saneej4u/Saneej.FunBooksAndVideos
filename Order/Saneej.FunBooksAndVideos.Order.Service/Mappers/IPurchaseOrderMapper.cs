using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.Mappers
{
    public interface IPurchaseOrderMapper
    {
        PurchaseOrderResponse MapOrderDetailsFromEntity(Data.Entities.PurchaseOrder purchaseOrderEntity);
    }
}
