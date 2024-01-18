namespace Saneej.FunBooksAndVideos.Service.Shipping
{
    public class ShippingService : IShippingService
    {
        public bool GenerateShippingSlip(int customerId, int orderId)
        {
            //var getAllPhysicalProduct = order.PurchaseOrderLines.Where(p => p.IsPhysicalProduct).ToList();
            return true;
        }
    }
}
