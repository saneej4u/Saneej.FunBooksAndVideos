using Saneej.FunBooksAndVideos.Order.Repository.UnitOfWork;
using Saneej.FunBooksAndVideos.Service.Constants;
using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.Customer
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MembershipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseWrapper<bool>> ActivateMembership(int customerId, int orderId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var order = await _unitOfWork.PurchaseOrderQueryRepository.FindByIdAsync(orderId, customerId);
                if (order == null)
                {
                    return ResponseWrapper.CreateClientError("Order not exist");
                }

                var purchasedMemberShip = order.PurchaseOrderLines.FirstOrDefault(p => GetMembershipCodes().Contains(p.ProductTypeCode));
                if (purchasedMemberShip == null)
                {
                    return ResponseWrapper.CreateClientError("There is no membership to activate");
                }

                var membsership = new Data.Entities.Membership
                {
                    CustomerId = customerId,
                    MembershipName = purchasedMemberShip.ProductTypeCode,
                    MemberShipStartedOn = DateTime.UtcNow,
                    MemberShipExpiresOn = DateTime.UtcNow.AddMonths(12),
                };

                await _unitOfWork.MembershipCommandRepository.AddMembership(membsership);

                await _unitOfWork.SaveChanges();

                _unitOfWork.CommitTransaction();

                // TODO : Inject mapper and return membership objects
                return ResponseWrapper.CreateSuccess(true);
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
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
