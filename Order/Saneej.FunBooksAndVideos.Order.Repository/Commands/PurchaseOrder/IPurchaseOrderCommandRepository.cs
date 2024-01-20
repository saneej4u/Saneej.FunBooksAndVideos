namespace Saneej.FunBooksAndVideos.Order.Repository.Commands.PurchaseOrder
{
    public interface IPurchaseOrderCommandRepository
    {
        Task AddOrder(Data.Entities.PurchaseOrder order);
    }
}
