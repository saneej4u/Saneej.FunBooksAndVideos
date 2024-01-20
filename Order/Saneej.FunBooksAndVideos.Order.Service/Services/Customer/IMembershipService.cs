namespace Saneej.FunBooksAndVideos.Service.Customer
{
    public interface IMembershipService
    {
        public Task<bool> ActivateMembership(int customerId, int orderId);
    }
}
