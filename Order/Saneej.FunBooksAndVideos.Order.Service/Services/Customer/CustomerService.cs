using Saneej.FunBooksAndVideos.Repository;
using Saneej.FunBooksAndVideos.Service.Constants;

namespace Saneej.FunBooksAndVideos.Service.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActivateMembership(int customerId, int orderId)
        {
            var order = await _unitOfWork.PurchaseOrderQueryRepository.FindByIdAsync(orderId, customerId);

            if (order is not null)
            {
                var purchasedMemberShip = order.PurchaseOrderLines.FirstOrDefault(p => GetMembershipCodes().Contains(p.ProductTypeCode));
                if (purchasedMemberShip != null)
                {
                    // TODO: update database
                    //Raise an event to activate customer account.
                }

                return true;
            }

            return false;
        }

        private static List<string> GetMembershipCodes()
        {
            return new List<string>
                    {
                        ProductTypeConstants.BOOK_MEMBERSHIP,
                        ProductTypeConstants.VIDEO_MEMBERSHIP,
                        ProductTypeConstants.PREMIUM_MEMBERSHIP
                    };
        }
    }
}
