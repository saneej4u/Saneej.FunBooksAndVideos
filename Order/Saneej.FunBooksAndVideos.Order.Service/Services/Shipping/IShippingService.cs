using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.Shipping
{
    public interface IShippingService
    {
        Task<ResponseWrapper<bool>> GenerateShippingSlip(int customerId, int orderId);
    }
}
