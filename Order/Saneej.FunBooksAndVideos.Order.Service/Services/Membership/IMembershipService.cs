using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.Customer
{
    public interface IMembershipService
    {
        public Task<ResponseWrapper<bool>> ActivateMembership(int customerId, int orderId);
    }
}
