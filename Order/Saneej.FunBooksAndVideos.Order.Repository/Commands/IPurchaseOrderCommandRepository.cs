using Saneej.FunBooksAndVideos.Data.Entities;

namespace Saneej.FunBooksAndVideos.Repository.Commands
{
    public interface IPurchaseOrderCommandRepository
    {
        Task AddOrder(PurchaseOrder order);
    }
}
