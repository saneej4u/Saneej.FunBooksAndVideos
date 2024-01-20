namespace Saneej.FunBooksAndVideos.Order.Repository.Commands.PurchaseOrder
{
    public interface IPurchaseOrderCommandRepository
    {
        Task AddPurchaseOrder(Data.Entities.PurchaseOrder order);
    }
}
