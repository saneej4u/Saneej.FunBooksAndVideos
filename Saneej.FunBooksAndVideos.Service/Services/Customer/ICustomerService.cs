namespace Saneej.FunBooksAndVideos.Service.Customer
{
    public interface ICustomerService
    {
        public bool ActivateMembership(int customerId, string membershipCode);
    }
}
