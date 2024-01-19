namespace Saneej.FunBooksAndVideos.Service.Customer
{
    public interface ICustomerService
    {
        public Task<bool> ActivateMembership(int customerId, int orderId);
    }
}
