namespace Saneej.FunBooksAndVideos.Order.Repository.Queries.PurchaseOrder
{
    public interface IPurchaseOrderQueryRepository
    {
        Task<Data.Entities.PurchaseOrder> FindByIdAsync(int orderId, int customerId);
    }
}
