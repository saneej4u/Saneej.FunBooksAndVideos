namespace Saneej.FunBooksAndVideos.Service.Shipping
{
    public interface IShippingService
    {
        Task<bool> GenerateShippingSlip(int customerId, int orderId);
    }
}
