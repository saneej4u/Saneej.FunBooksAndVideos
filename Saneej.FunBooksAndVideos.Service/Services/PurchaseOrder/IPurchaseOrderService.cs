using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.PurchaseOrder
{
    public interface IPurchaseOrderService
    {
        Task<ResponseWrapper<PurchaseOrderResponse>> ProcessOrder(BasketRequest basket);
    }
}
