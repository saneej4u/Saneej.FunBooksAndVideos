namespace Saneej.FunBooksAndVideos.Service.Shipping
{
    public interface IShippingService
    {
        public bool GenerateShippingSlip(int customerId, int orderId);
    }
}
