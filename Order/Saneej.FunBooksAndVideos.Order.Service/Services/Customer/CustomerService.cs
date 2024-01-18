using Saneej.FunBooksAndVideos.Service.Constants;

namespace Saneej.FunBooksAndVideos.Service.Customer
{
    public class CustomerService : ICustomerService
    {
        public bool ActivateMembership(int customerId, string membershipCode)
        {
            return true;
        }
    }
}
